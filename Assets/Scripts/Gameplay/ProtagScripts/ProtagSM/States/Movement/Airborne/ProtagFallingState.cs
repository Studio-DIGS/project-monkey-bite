using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagFallingState : ProtagState
{
    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        context.coyoteTimer += Time.deltaTime;

        transitions.ToProtagStateSelector();
    }

    public override void FixedUpdateState()
    {
        playerSimplePathMovement.SimpleAirborneHorizontalMovement(
            inputState.horizontalAxis,
            hMoveProfile.airborneWalkVel,
            hMoveProfile.airborneWalkAccel,
            hMoveProfile.airborneFriction,
            Time.fixedDeltaTime,
            controllerMotor.CurrentGroundState.GroundNormal);
    }

    
}
