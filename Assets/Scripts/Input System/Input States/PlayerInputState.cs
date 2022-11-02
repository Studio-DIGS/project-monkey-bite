using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputState
{
    // Events (Yes I know this is cursed as fuck)
    public Action OnJumpPressed;
    public Action OnJumpReleased;

    // Values
    public float horizontalAxis;
    public Vector2 mousePosition;

    // Conditionals
    public bool canJump = true;

    // Buttons
    public bool jumpDown;
}
