using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Architecture/Input/InputSources/UserInput/GameplayMapInputSource")]
public class GameplayMapInputSource : ActionMapInputSource<PlayerInputState>, DefaultControls.IGameplayActions
{
    public delegate void SimpleInputAction(InputAction.CallbackContext context);

    public override void SetupCallbacks(DefaultControls controlScheme)
    {
        controlScheme.Gameplay.SetCallbacks(this);
    }

    public override PlayerInputState ProcessInputState(PlayerInputState data)
    {
        data.horizontalAxis = horizontalMovementContext.ReadValue<float>();
        data.mousePosition = mousePosition;
        return data;
    }

    // Frame based state fields

    private InputAction.CallbackContext horizontalMovementContext;
    private Vector2 mousePosition;

    // Event fields

    public event SimpleInputAction jumpEvent;

    // Frame based  callbacks

    public void OnHorizontalMovement(InputAction.CallbackContext context)
    {
        horizontalMovementContext = context;
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    // Event based callbacks
    public void OnJump(InputAction.CallbackContext context)
    {
        jumpEvent?.Invoke(context);
    }
}
