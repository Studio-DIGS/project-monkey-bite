using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ActionMapInputSource<T> : ScriptableObject, InputSource<T>
{
    public abstract T ProcessInputState(T data);
    public abstract void SetupCallbacks(DefaultControls controlScheme);
}
