using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class KeyActionDictionary
{
    private readonly Dictionary<KeyCode, Action<InputState>> legacyInputActions = new();
    private readonly Dictionary<string, Action<InputState>> inputSystemActions = new();
    private InputActionAsset inputActionAsset;

    public void Init(InputActionAsset inputActionAsset)
    {
        this.inputActionAsset = inputActionAsset;
    }

    public void StartListen(KeyCode keycode, Action<InputState> action)
    {
        if (!legacyInputActions.TryAdd(keycode, action)) legacyInputActions[keycode] += action;
    }

    public void StartListen(KeyActionType type, Action<InputState> action)
    {
        string key = type.ToString();
        if (!inputSystemActions.TryAdd(key, action)) inputSystemActions[key] += action;
    }

    public void StopListen(KeyCode keycode, Action<InputState> action)
    {
        if (legacyInputActions.ContainsKey(keycode)) legacyInputActions[keycode] -= action;
    }

    public void StopListen(KeyActionType type, Action<InputState> action)
    {
        string key = type.ToString();
        if (inputSystemActions.ContainsKey(key)) inputSystemActions[key] -= action;
    }

    public void Check()
    {
        Dictionary<KeyCode, Action<InputState>> legacy = new(legacyInputActions);
        Dictionary<string, Action<InputState>> inputSystem = new(inputSystemActions);

        foreach (var pair in legacy)
        {
            Check(pair.Key, pair.Value);
        }

        foreach (var pair in inputSystem)
        {
            Check(pair.Key, pair.Value);
        }
    }

    private void Check(KeyCode keycode, Action<InputState> action)
    {
        InputState inputState;

        if (Input.GetKeyDown(keycode)) inputState = InputState.Down;
        else if (Input.GetKeyUp(keycode)) inputState = InputState.Up;
        else if (Input.GetKey(keycode)) inputState = InputState.Hold;
        else inputState = InputState.None;

        action?.Invoke(inputState);
    }

    private void Check(string key, Action<InputState> action)
    {
        foreach (InputControl control in inputActionAsset.FindAction(key).controls)
        {
            InputState inputState = InputState.None;
            if (control is ButtonControl button)
            {
                if (button.wasPressedThisFrame) inputState = InputState.Down;
                else if (button.wasReleasedThisFrame) inputState = InputState.Up;
                else if (button.isPressed) inputState = InputState.Hold;
            }
            else if (control is AxisControl axis)
            {
                float value = axis.ReadValue();
                if (Mathf.Abs(value) > 0.1f) inputState = InputState.Down;
            }
            action?.Invoke(inputState);
        }
    }
}
