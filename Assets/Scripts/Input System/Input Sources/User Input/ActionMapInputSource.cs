using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class ActionMapInputSource<InputState> : ScriptableObject, InputSource<InputState>
{
    public abstract InputState ProcessInputState(InputState data);
    public abstract void SetCallbacks(IInputActionCollection2 assetWrapper);
}

[System.Serializable]
public class ActionMapSourceEvent : UnityEvent<InputAction.CallbackContext> { }