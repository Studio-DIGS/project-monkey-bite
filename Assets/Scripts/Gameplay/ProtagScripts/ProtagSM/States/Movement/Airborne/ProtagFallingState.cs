using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagFallingState : ProtagState
{
    public override void EnterState()
    {
        animationController.Play("Fall");
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState(float deltaTime)
    {
        context.coyoteTimer += deltaTime;

        transitions.ToProtagStateSelector();
    }

    public override void FixedUpdateState(float fixedDeltaTime)
    {
        playerSimplePathMovement.SimpleAirborneHorizontalMovement(
            inputState.horizontalAxis,
            hMoveProfile.airborneWalkVel,
            hMoveProfile.airborneWalkAccel,
            hMoveProfile.airborneFriction,
            fixedDeltaTime,
            controllerMotor.CurrentGroundState.GroundNormal);
    }

    
}
