
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerUserInputProvider", menuName = "Architecture/Input/Providers/PlayerUserInputProvider")]

public class PlayerUserInputProvider : DescriptionBaseSO, InputProvider<PlayerInputState, PlayerInputEvents>
{
    [Header("Gameplay Actions")]
    
    [SerializeField] private InputActionReference horizontalMovement;
    [SerializeField] private InputActionReference mousePosition;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference dodge;
    [SerializeField] private InputActionReference pause;
    [SerializeField] private InputActionReference interact;
    [SerializeField] private InputActionReference mainAttack;
    [SerializeField] private InputActionReference altAttack;

    private GameplayCommandBuffer commandBuffer;
    public GameplayCommandBuffer GameplayCommandBuffer => commandBuffer;

    private PlayerInputEvents events;
    public PlayerInputEvents Events => events;

    private PlayerInputState _state;

    private static float inputCommandDuration = 0.1f;

    public enum PlayerCommandID
    {
        JumpCommandDown,
        JumpCommandUp,
        PrimaryAttackCommandDown,
        PrimaryAttackCommandUp,
        SecondaryAttackCommandDown,
        SecondaryAttackCommandUp,
        DodgeCommandDown,
        DodgeCommandUp
    }

    [Flags]
    public enum PlayerCommandFlag
    {
        CombatCommand = 0,
        MovementCommand = 1,
    }

    private void OnEnable()
    {
        commandBuffer = new GameplayCommandBuffer();
        events = new PlayerInputEvents();
        
        // Link up events with unity Input System
        horizontalMovement.action.SetActionCallbacks(OnHorizontalMovement);
        mousePosition.action.SetActionCallbacks(OnMousePosition);
        jump.action.SetActionCallbacks(OnJump);
        dodge.action.SetActionCallbacks(OnDodge);
        pause.action.SetActionCallbacks(OnPause);
        interact.action.SetActionCallbacks(OnInteract);
        mainAttack.action.SetActionCallbacks(OnMainAttack);
        altAttack.action.SetActionCallbacks(OnAltAttack);
    }

    public void GetInputState(ref PlayerInputState state)
    {
        state.horizontalAxis = horizontalMovement.action.ReadValue<float>();
        state.mousePosition = mousePosition.action.ReadValue<Vector2>();
        state.jumpHeld = jump.action.ReadValue<float>() > 0;
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
            commandBuffer.BufferGameplayCommand(
                (int)PlayerCommandID.JumpCommandDown, 
                true, 
                inputCommandDuration,
                (int)PlayerCommandFlag.MovementCommand);
        
        if (context.canceled)
            commandBuffer.BufferGameplayCommand(
                (int)PlayerCommandID.JumpCommandUp, 
                false, 
                inputCommandDuration,
                (int)PlayerCommandFlag.MovementCommand);
    }
    
    private void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
            commandBuffer.BufferGameplayCommand(
                (int)PlayerCommandID.DodgeCommandDown, 
                true, 
                inputCommandDuration,
                (int)PlayerCommandFlag.MovementCommand);
        
        if (context.canceled)
            commandBuffer.BufferGameplayCommand(
                (int)PlayerCommandID.DodgeCommandUp, 
                false, 
                inputCommandDuration,
                (int)PlayerCommandFlag.MovementCommand);
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
            commandBuffer.BufferGameplayCommand(
                (int)PlayerCommandID.PrimaryAttackCommandDown, 
                false, 
                inputCommandDuration,
                (int)PlayerCommandFlag.CombatCommand);
        
        if (context.canceled)
            commandBuffer.BufferGameplayCommand(
                (int)PlayerCommandID.PrimaryAttackCommandUp, 
                false, 
                inputCommandDuration,
                (int)PlayerCommandFlag.CombatCommand);
    }


    public void OnAltAttack(InputAction.CallbackContext context)
    {
        if (context.started)
            commandBuffer.BufferGameplayCommand(
                (int)PlayerCommandID.SecondaryAttackCommandDown, 
                false, 
                inputCommandDuration,
                (int)PlayerCommandFlag.CombatCommand);
        
        if (context.canceled)
            commandBuffer.BufferGameplayCommand(
                (int)PlayerCommandID.SecondaryAttackCommandUp, 
                false, 
                inputCommandDuration,
                (int)PlayerCommandFlag.CombatCommand);
    }
}

