using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoSingleton<InputManager>
{
    [SerializeField] private InputActionAsset InputActionAsset;

    private KeyActionDictionary dictionary;
    private bool inputEnabled;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void InitStatic()
    {
        ShuttingDown = false;
    }

    private void Awake()
    {
        dictionary = new KeyActionDictionary();
        dictionary.Init(InputActionAsset);

        EventManager.StartListening(Event.DisableInput, DisableInput);
        EventManager.StartListening(Event.EnableInput, EnableInput);
    }

    private void Update()
    {
        if (inputEnabled) dictionary.Check();
    }

    public void StartListen(KeyCode keycode, Action<InputState> action) => dictionary.StartListen(keycode, action);
    public void StartListen(KeyActionType type, Action<InputState> action) => dictionary.StartListen(type, action);
    public void StopListen(KeyCode keycode, Action<InputState> action) => dictionary.StopListen(keycode, action);
    public void StopListen(KeyActionType type, Action<InputState> action) => dictionary.StopListen(type, action);

    private void DisableInput() => inputEnabled = false;
    private void EnableInput() => inputEnabled = true;

    private void OnDestroy()
    {
        EventManager.StopListening(Event.DisableInput, DisableInput);
        EventManager.StopListening(Event.EnableInput, EnableInput);
    }
}
