using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagIdleState : ProtagState
{
    public override void EnterState()
    {
        context.coyoteTimer = 0f;
        animationController.Play("Idle");
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
        Vector2 groundNormal = controllerMotor.currentGroundState.groundNormal;

        playerSimplePathMovement.SimpleGroundedHorizontalMovement(
            0, 
            hMoveProfile.groundedWalkVel,
            hMoveProfile.groundedWalkAccel,
            hMoveProfile.groundedFriction,
            Time.fixedDeltaTime, 
            groundNormal);
    }
}
