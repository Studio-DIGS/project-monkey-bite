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

    public override void UpdateState(float deltaTime)
    {
        transitions.ToProtagStateSelector();
    }

    public override void FixedUpdateState(float fixedDeltaTime)
    {
        Vector2 groundNormal = controllerMotor.CurrentGroundState.GroundNormal;

        playerSimplePathMovement.SimpleGroundedHorizontalMovement(
            0, 
            hMoveProfile.groundedWalkVel,
            hMoveProfile.groundedWalkAccel,
            hMoveProfile.groundedFriction,
            fixedDeltaTime, 
            groundNormal);
    }
}
