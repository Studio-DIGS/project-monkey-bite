using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Architecture/Input/InputProvider/PlayerGameplayProvider")]
public class PlayerGameplayInputProvider : InputProvider<PlayerInputState>
{
    public List<ActionMapInputSource<PlayerInputState>> userInputSources;
    public UIInputSource UIInputSource;

    public override PlayerInputState GetInputState()
    {     
        inputState ??= new PlayerInputState();
        foreach(ActionMapInputSource<PlayerInputState> source in userInputSources)
        {
            inputState = source.ProcessInputState(inputState);
        }
        inputState = UIInputSource.ProcessInputState(inputState);
        return inputState;
    }


    public void HandleOnJump(InputAction.CallbackContext context)
    {
        if(inputState.canJump)
        {
            HandleInputEvent(context, inputState.OnJumpPressed, null, inputState.OnJumpReleased);
        }
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
