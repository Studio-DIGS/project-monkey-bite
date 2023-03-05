using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagWalkingState : ProtagState
{

    public override void EnterState()
    {
        WalkMovement();
        context.coyoteTimer = 0f;
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        transitions.ToProtagStateSelector();
    }

    public override void FixedUpdateState()
    {
        WalkMovement();
    }

    private void WalkMovement()
    {
        Vector2 groundNormal = movementContext.SurfaceNormal;

        playerSimplePathMovement.SimpleGroundedHorizontalMovement(
            inputState.horizontalAxis, 
            hMoveProfile.groundedWalkVel,
            hMoveProfile.groundedWalkAccel,
            hMoveProfile.groundedFriction,
            Time.fixedDeltaTime, 
            groundNormal);
    }
}
