using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Architecture/Input/InputProvider/PlayerGameplayProvider")]
public class PlayerGameplayInputProvider : PlayerInputProvider
{
    public GameplayMapInputSource userInputSource;
    public UIInputSource UIInputSource;

    private DefaultControls controls;

    public override PlayerInputState GetInputState()
    {
        inputState = new PlayerInputState();
        inputState = userInputSource.ProcessInputState(inputState);
        inputState = UIInputSource.ProcessInputState(inputState);
        return inputState;
    }

    public override void Setup()
    {
        controls = new DefaultControls();
        controls.Gameplay.Enable();
        userInputSource.SetupCallbacks(controls);
        SubscribeEvents();
    }

    public override void Cleanup()
    {
        controls.Dispose();
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        userInputSource.jumpEvent += HandleOnJump;
    }

    private void UnsubscribeEvents()
    {
        userInputSource.jumpEvent -= HandleOnJump;
    }

    // Event handlers

    private void HandleOnJump(InputAction.CallbackContext context)
    {
        if(inputState.canJump)
            HandleInputEvent(context, OnJumpPressed, null, OnJumpReleased);
    }

    private void HandleInputEvent(InputAction.CallbackContext context, Action started = null, Action performed = null, Action canceled = null)
    {
        if (context.started)
            started?.Invoke();
        else if (context.performed)
            performed?.Invoke();
        else
            canceled?.Invoke();
    }

}
