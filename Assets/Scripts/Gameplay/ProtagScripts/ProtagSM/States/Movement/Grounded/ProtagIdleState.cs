using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagIdleState : ProtagState
{
    public override void EnterState()
    {
        context.coyoteTimer = 0f;
    }

    public override void ExitState()
    {
        pathBody.constrainVelocity = false;
    }

    public override void UpdateState()
    {
        transitions.ToProtagStateSelector();
    }

    public override void FixedUpdateState()
    {
        pathBody.pathVelocity += playerSimplePathMovement.CalculateHorizontalFrictionStep(
            hMoveProfile.groundedFriction,
            Time.fixedDeltaTime, 
            movementContext.SurfaceNormal);

        if(pathBody.pathVelocity.magnitude < 0.5f)
            pathBody.constrainVelocity = true;
    }
}
