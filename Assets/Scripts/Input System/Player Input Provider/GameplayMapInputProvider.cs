
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "GameplayMapInputProvider", menuName = "Architecture/Input/Providers/GameplayMapInputProvider")]
public class GameplayMapInputProvider : PlayerInputStateProvider
{
    // References
    [SerializeField] private InputActionReference horizontalMovement;
    [SerializeField] private InputActionReference mousePosition;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference pause;

    public override PlayerInputState GetInputState(PlayerInputState state)
    {
        state ??= new PlayerInputState();
        state.horizontalAxis = horizontalMovement.action.ReadValue<float>();
        state.mousePosition = mousePosition.action.ReadValue<Vector2>();
        return state;
    }

    public override void SetupEvents()
    {
        horizontalMovement.action.SetCallbacks(OnHorizontalMovement);
        mousePosition.action.SetCallbacks(OnMousePosition);
        jump.action.SetCallbacks(OnJump);
        pause.action.SetCallbacks(OnPause);
    }

    // Callback Listeners

    private void OnHorizontalMovement(InputAction.CallbackContext context)
    {
        
    }
    
    private void OnMousePosition(InputAction.CallbackContext context)
    {
        
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed) 
            OnJumpPressed?.Invoke();
        if(context.canceled) 
            OnJumpReleased?.Invoke();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if(context.started)
            OnPausePressed?.Invoke();
    }
}
