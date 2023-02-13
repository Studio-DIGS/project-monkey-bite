using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class PlayerJumpingState : PlayerMovementState
{
    public PlayerJumpingState(StateMachine<PlayerBlackboard> stateMachine) : base(stateMachine)
    {
        
    }
    
    public override bool TryTransition(ref State<PlayerBlackboard> c)
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
            c = GetState<PlayerFallingState>();
            return true;
        }
    
        return false;
    }

    public override void EnterState()
    {
        pathBody.pathVelocity.y = movementProfile.jumpStrength;
        pathBody.SetGravityEnabled(false);
        blackboard.coyoteTimer = float.MaxValue;
        
        transitions.AddOnJumpPressedToFootstoolJump();
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
        if (!movementContextController.IsOnSurface)
        {
            playerSimplePathMovement.SimpleAirborneHorizontalMovement(
                inputState.horizontalAxis, 
                movementProfile.airborneWalkVel,
                movementProfile.airborneWalkAccel,
                movementProfile.airborneFriction,
                Time.fixedDeltaTime, 
                movementContextController.SurfaceNormal);
        }
    }

 
}
