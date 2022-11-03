using System;
using UnityEngine.InputSystem;
using UnityEngine;

public abstract class PlayerInputStateProvider : ScriptableObject, InputProvider<PlayerInputState>
{
    // Events
    public Action OnJumpPressed;
    public Action OnJumpReleased;
    public Action OnPausePressed;
    
    public abstract PlayerInputState GetInputState(PlayerInputState source = null);
    public abstract void SetupEvents();
}
