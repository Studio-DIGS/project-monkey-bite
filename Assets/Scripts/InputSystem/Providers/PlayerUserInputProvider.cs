
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerUserInputProvider", menuName = "Architecture/Input/Providers/PlayerUserInputProvider")]

public class PlayerUserInputProvider : ScriptableObject, InputProvider<PlayerInputState, PlayerInputEvents>
{
    [Header("Gameplay Actions")]
    
    [SerializeField] private InputActionReference horizontalMovement;
    [SerializeField] private InputActionReference mousePosition;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference pause;
    [SerializeField] private InputActionReference interact;
    [SerializeField] private InputActionReference mainAttack;
    [SerializeField] private InputActionReference altAttack;
    
    private PlayerInputEvents events;
    public PlayerInputEvents Events => events;

    private PlayerInputState _state;

    private void OnEnable()
    {
        events = new PlayerInputEvents();
        // Link up events with unity Input System
        horizontalMovement.action.SetActionCallbacks(OnHorizontalMovement);
        mousePosition.action.SetActionCallbacks(OnMousePosition);
        jump.action.SetActionCallbacks(OnJump);
        pause.action.SetActionCallbacks(OnPause);
        interact.action.SetActionCallbacks(OnInteract);
        mainAttack.action.SetActionCallbacks(OnMainAttack);
        altAttack.action.SetActionCallbacks(OnAltAttack);
    }

    public void GetInputState(ref PlayerInputState state)
    {
        //state ??= new PlayerInputState();
        state.horizontalAxis = horizontalMovement.action.ReadValue<float>();
        state.mousePosition = mousePosition.action.ReadValue<Vector2>();
        state.jumpHeld = jump.action.ReadValue<float>() > 0;
        //return state;
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
        if (context.performed)
            events.OnJumpPressed?.Invoke();
        if (context.canceled)
            events.OnJumpReleased?.Invoke();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
            events.OnPausePressed?.Invoke();
    }


    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
            events.OnInteractPressed?.Invoke();
    }


    public void OnMainAttack(InputAction.CallbackContext context)
    {
        if (context.started)
            events.OnMainAttackPressed?.Invoke();
    }


    public void OnAltAttack(InputAction.CallbackContext context)
    {
        if (context.started)
            events.OnAltAttackPressed?.Invoke();
    }
}

