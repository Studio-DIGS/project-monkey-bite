using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class PlayerJumpingState : PlayerMovementState
{
    public PlayerJumpingState(StateMachine<PlayerBlackboard> stateMachine) : base(stateMachine)
    {
    }

    private float jumpTime;

    public override State<PlayerBlackboard> GetSwitchState()
    {
        bool minTimePassed = jumpTime > movementProfile.minJumpTime;
        if (movementContextController.IsGrounded && minTimePassed)
        {
            return GetState<PlayerWalkingState>();
        }

        if (minTimePassed && (jumpTime > movementProfile.maxJumpTime || !inputState.jumpHeld))
        {
            return GetState<PlayerFallingState>();
        }

        return null;
    }
    
    public override void EnterState()
    {
        jumpTime = 0f;
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
        jumpTime += Time.fixedDeltaTime;
        
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
