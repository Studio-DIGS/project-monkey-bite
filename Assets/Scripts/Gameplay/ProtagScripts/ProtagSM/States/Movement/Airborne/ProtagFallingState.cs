using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagFallingState : ProtagState
{
    public override bool TryTransition(ref State<ProtagBlackboard> c)
    {
        return moveTransitions.ToProtagStateSelector(ref c);
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        context.coyoteTimer += Time.deltaTime;
    }

    public override void FixedUpdateState()
    {
        if (!movementContext.IsOnSurface)
        {
            playerSimplePathMovement.SimpleAirborneHorizontalMovement(
                inputState.horizontalAxis,
                hMoveProfile.airborneWalkVel,
                hMoveProfile.airborneWalkAccel,
                hMoveProfile.airborneFriction,
                Time.fixedDeltaTime,
                movementContext.SurfaceNormal);
        }
    }

    
}
