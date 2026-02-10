using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ObjectResolver
{
    private readonly HashSet<Type> registrations = new();
    private readonly Dictionary<Type, object> instancePerTypeDict = new();

    /// <summary>
    /// Register a type so it can be constructed
    /// </summary>
    public void Register<T>()
    {
        registrations.Add(typeof(T));
    }

    /// <summary>
    /// Register an existing instance
    /// </summary>
    public void RegisterInstance<T>(T instance)
    {
        instancePerTypeDict[typeof(T)] = instance;
        registrations.Add(instance.GetType());
    }

    /// <summary>
    /// Unregister a type
    /// </summary>
    public void UnregisterType<T>()
    {
        instancePerTypeDict.Remove(typeof(T));
        registrations.Remove(typeof(T));
    }

    /// <summary>
    /// Get corresponding instance for given type
    /// </summary>
    /// <returns>Instance of the type</returns>
    public T Resolve<T>()
    {
        return (T)Resolve(typeof(T), new Stack<Type>());
    }

    /// <summary>
    /// Core Resolve
    /// </summary>
    /// <returns>Instance of the type or null if there isn't one</returns>
    private object Resolve(Type type, Stack<Type> stack)
    {
        // try get from cache
        if (instancePerTypeDict.TryGetValue(type, out object instance))
        {
            return instance;
        }

        // block circular dependencies
        if (stack.Contains(type))
        {
            string cycle = string.Join(" -> ", stack.Reverse().Select(t => t.Name).Concat(new[] { type.Name }));
            Debug.LogError($"[{this}] Circular dependency detected: {cycle}");
            return null;
        }

        // ensure type is registered
        if (!registrations.Contains(type))
        {
            Debug.LogError($"[{this}] Couldn't resolve type {type}");
            return null;
        }

        // get constructor
        ConstructorInfo constructor;
        try
        {
            constructor = type.GetConstructors().Single();
        }
        catch (InvalidOperationException e)
        {
            Debug.LogError($"[{this}] Couldn't resolve constructor for type {type}\n{e}");
            return null;
        }

        // resolve dependencies
        object[] args;
        stack.Push(type);
        try
        {
            ParameterInfo[] parameters = constructor.GetParameters();
            args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                Type pType = parameters[i].ParameterType;
                object dependency = Resolve(pType, stack);
                if (dependency == null)
                {
                    Debug.LogError($"[{this}] Failed to resolve dependency for type {pType}");
                    return null;
                }
                args[i] = dependency;
            }
        }
        finally
        {
            stack.Pop();
        }

        // create instance and cache
        try
        {
            instance = Activator.CreateInstance(type, args);
        }
        catch (Exception e)
        {
            Debug.LogError($"[{this}] Failed to create instance for {type}\n{e}");
            throw;
        }

        instancePerTypeDict[type] = instance;
        return instance;
    }
}