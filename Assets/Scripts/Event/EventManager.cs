using System;
using System.Collections.Generic;

public static class EventManager
{
    private static readonly Dictionary<Event, Action> Events = new();

    public static void StartListening(Event eventName, Action listener)
    {
        if (!Events.TryAdd(eventName, listener)) Events[eventName] += listener;
    }

    public static void StopListening(Event eventName, Action listener)
    {
        if (Events.ContainsKey(eventName)) Events[eventName] -= listener;
    }

    public static void TriggerEvent(Event eventName)
    {
        if (Events.TryGetValue(eventName, out Action listeners)) listeners?.Invoke();
    }
}

public static class EventManager<T>
{
    private static readonly Dictionary<Event, Action<T>> Events = new();

    public static void StartListening(Event eventName, Action<T> listener)
    {
        if (!Events.TryAdd(eventName, listener)) Events[eventName] += listener;
    }

    public static void StopListening(Event eventName, Action<T> listener)
    {
        if (Events.ContainsKey(eventName)) Events[eventName] -= listener;
    }

    public static void TriggerEvent(Event eventName, T param)
    {
        if (Events.TryGetValue(eventName, out Action<T> listeners)) listeners.Invoke(param);
    }
}