using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagJumpingState : ProtagState
{
    public ProtagJumpingState(StateMachine<ProtagBlackboard> stateMachine) : base(stateMachine)
    {
        
    }
    
    public override bool TryTransition(ref State<ProtagBlackboard> c)
    {
        
        float jumpTime = Time.time - stateEntryTime;
        bool minTimePassed = jumpTime > movementProfile.minJumpTime;
        bool maxTimePassed = jumpTime > movementProfile.maxJumpTime;
        
        if (minTimePassed && transitions.WhenGroundedToWalk(ref c))
        {
            return true;
        }
        if (minTimePassed && (maxTimePassed || !inputState.jumpHeld))
        {
            c = GetState<ProtagFallingState>();
            return true;
        }
    
        return false;
    }

    public override void EnterState()
    {
        pathBody.pathVelocity.y = movementProfile.jumpStrength;
        pathBody.SetGravityEnabled(false);
        blackboard.coyoteTimer = float.MaxValue;
    }

    public override void ExitState()
    {
        pathBody.pathVelocity.y = Mathf.MoveTowards(
            pathBody.pathVelocity.y,
            0f,
            movementProfile.jumpEndVel);
        
        pathBody.SetGravityEnabled(true);
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        pathBody.pathVelocity.y = movementProfile.jumpStrength;
        if (!movementContext.IsOnSurface)
        {
            playerSimplePathMovement.SimpleAirborneHorizontalMovement(
                inputState.horizontalAxis, 
                movementProfile.airborneWalkVel,
                movementProfile.airborneWalkAccel,
                movementProfile.airborneFriction,
                Time.fixedDeltaTime, 
                movementContext.SurfaceNormal);
        }
    }

 
}
