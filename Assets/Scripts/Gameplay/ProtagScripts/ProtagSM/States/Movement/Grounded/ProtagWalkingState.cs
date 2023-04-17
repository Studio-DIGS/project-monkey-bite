using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagWalkingState : ProtagState
{

    public override void EnterState()
    {
        context.coyoteTimer = 0f;
        animationController.Play("Run");
    }

    public override void ExitState()
    {
    }

    public override void UpdateState(float deltaTime)
    {
        // Enable turning
        AlignCharacter();
        
        transitions.ToProtagStateSelector();
    }

    public override void FixedUpdateState(float fixedDeltaTime)
    {
        Vector2 groundNormal = controllerMotor.CurrentGroundState.GroundNormal;

        playerSimplePathMovement.SimpleGroundedHorizontalMovement(
            inputState.horizontalAxis, 
            hMoveProfile.groundedWalkVel,
            hMoveProfile.groundedWalkAccel,
            hMoveProfile.groundedFriction,
            fixedDeltaTime, 
            groundNormal);
    }
}
