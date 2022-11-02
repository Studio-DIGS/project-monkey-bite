using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Architecture/Input/InputSources/UserInput/GameplayMapInputSource")]
public class GameplayMapInputSource : ActionMapInputSource<PlayerInputState>, DefaultControls.IGameplayActions
{
    private DefaultControls.GameplayActions actionMap;

    public override void SetCallbacks(IInputActionCollection2 controlScheme)
    {
        actionMap = ((DefaultControls)controlScheme).Gameplay;
        actionMap.SetCallbacks(this);
    }

    public override PlayerInputState ProcessInputState(PlayerInputState data)
    {
        data.horizontalAxis = actionMap.HorizontalMovement.ReadValue<float>();
        data.mousePosition = actionMap.MousePosition.ReadValue<Vector2>();
        return data;
    }

    // Event fields

    public ActionMapSourceEvent jumpEvent;
    public ActionMapSourceEvent horizontalMovementEvent;
    public ActionMapSourceEvent mousePositionEvent;

    // Callbacks

    public void OnHorizontalMovement(InputAction.CallbackContext context)
    {
        horizontalMovementEvent?.Invoke(context);
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        mousePositionEvent?.Invoke(context);    
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpEvent?.Invoke(context);
    }
}
