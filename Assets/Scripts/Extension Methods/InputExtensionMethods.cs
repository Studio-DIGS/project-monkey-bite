using System;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputExtensionMethods
{
    public static void SetCallbacks(this InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.started += callback;
        action.performed += callback;
        action.canceled += callback;
    }
    
    public static void RemoveCallbacks(this InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.started -= callback;
        action.performed -= callback;
        action.canceled -= callback;
    }
}
