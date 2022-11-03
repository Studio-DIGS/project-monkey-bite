
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "GameplayMapInputProvider", menuName = "Architecture/Input/Providers/GameplayMapInputProvider")]
public class GameplayMapInputProvider : PlayerInputStateProvider
{
    // Persistent state

    private InputAction.CallbackContext horizontalMovement;
    private InputAction.CallbackContext mousePosition;

    public override PlayerInputState GetInputState()
    {
        var state = new PlayerInputState();
        state.horizontalAxis = horizontalMovement.ReadValue<float>();
        state.mousePosition = mousePosition.ReadValue<Vector2>();
        return state;
    }
    
    // Callback Listeners

    public void OnHorizontalMovement(InputAction.CallbackContext context)
    {
        horizontalMovement = context;
    }
    
    public void OnMousePosition(InputAction.CallbackContext context)
    {
        mousePosition = context;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed) 
            OnJumpPressed?.Invoke();
        if(context.canceled) 
            OnJumpReleased?.Invoke();
    }
}
