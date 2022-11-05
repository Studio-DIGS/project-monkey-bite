
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "UserInputProvider", menuName = "Architecture/Input/Providers/GameplayMapInputProvider")]

public class UserPInputStateProviderSO : PInputStateProviderSO
{
    [Header("Gameplay Actions")] [SerializeField]
    private InputActionReference horizontalMovement;

    [SerializeField] private InputActionReference mousePosition;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference pause;
    [SerializeField] private InputActionReference interact;
    [SerializeField] private InputActionReference mainAttack;
    [SerializeField] private InputActionReference altAttack;

    private void OnEnable()
    {
        horizontalMovement.action.SetActionCallbacks(OnHorizontalMovement);
        mousePosition.action.SetActionCallbacks(OnMousePosition);
        jump.action.SetActionCallbacks(OnJump);
        pause.action.SetActionCallbacks(OnPause);
        interact.action.SetActionCallbacks(OnInteract);
        mainAttack.action.SetActionCallbacks(OnMainAttack);
        altAttack.action.SetActionCallbacks(OnAltAttack);
    }

    public override PlayerInputState GetInputState(PlayerInputState state)
    {
        state ??= new PlayerInputState();
        state.horizontalAxis = horizontalMovement.action.ReadValue<float>();
        state.mousePosition = mousePosition.action.ReadValue<Vector2>();
        return state;
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
            OnJumpPressed?.Invoke();
        if (context.canceled)
            OnJumpReleased?.Invoke();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
            OnPausePressed?.Invoke();
    }


    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
            OnInteractPressed?.Invoke();
    }


    public void OnMainAttack(InputAction.CallbackContext context)
    {
        if (context.started)
            OnMainAttackPressed?.Invoke();
    }


    public void OnAltAttack(InputAction.CallbackContext context)
    {
        if (context.started)
            OnAltAttackPressed?.Invoke();
    }
}

