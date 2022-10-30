using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerInputProvider : ScriptableObject
{
    protected PlayerInputState inputState;

    // Events
    public Action OnJumpPressed;
    public Action OnJumpReleased;

    public abstract PlayerInputState GetInputState();
    public abstract void Setup();
    public abstract void Cleanup();
}
