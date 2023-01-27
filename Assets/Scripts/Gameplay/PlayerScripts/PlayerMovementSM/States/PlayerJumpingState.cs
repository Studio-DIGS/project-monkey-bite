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
        if (movementContextController.IsGrounded && jumpTime > 0.15f)
        {
            return GetState<PlayerWalkingState>();
        }

        if (jumpTime > movementProfile.maxJumpTime || !inputState.jumpHeld)
        {
            return GetState<PlayerFallingState>();
        }

        return null;
    }
    
    public override void EnterState()
    {
        jumpTime = 0f;
        pathBody.pathVelocity.y = movementProfile.jumpStrength;
    }

    public override void ExitState()
    {
        pathBody.pathVelocity.y = Mathf.MoveTowards(
            pathBody.pathVelocity.y,
            0f,
            movementProfile.jumpEndVel);
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        jumpTime += Time.fixedDeltaTime;
        
        pathBody.pathVelocity.y = movementProfile.jumpStrength;
        
        playerSimplePathMovement.SimpleHorizontalMovement(
            inputState.horizontalAxis, 
            movementProfile.airborneWalkVel,
            movementProfile.airborneWalkAccel,
            movementProfile.airborneFriction,
            Time.fixedDeltaTime, 
            Vector3.up);
    }

 
}
