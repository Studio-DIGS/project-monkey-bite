using System;
using UnityEngine.InputSystem;
using UnityEngine;

public abstract class PlayerInputStateProvider : ScriptableObject, InputProvider<PlayerInputState>
{
    // Events
    public Action OnJumpPressed;
    public Action OnJumpReleased;
    public abstract PlayerInputState GetInputState();
}
