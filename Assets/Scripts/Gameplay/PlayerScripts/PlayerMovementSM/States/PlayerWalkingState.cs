using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class PlayerWalkingState : PlayerMovementState
{
    public PlayerWalkingState(StateMachine<PlayerBlackboard> stateMachine) : base(stateMachine)
    {
        
    }

    public override State<PlayerBlackboard> GetSwitchState()
    {
        if (inputState.jumpPressed)
        {
            return GetState<PlayerJumpingState>();
        }
        if (!movementContextController.IsGrounded)
        {
            return GetState<PlayerFallingState>();
        }
        if (blackboard.inputState.horizontalAxis == 0)
        {
            return GetState<PlayerIdleState>();
        }

        return null;
    }
    
    public override void EnterState()
    {
        WalkMovement();
        blackboard.coyoteTimer = 0f;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        WalkMovement();
    }

    private void WalkMovement()
    {
        Vector2 groundNormal = movementContextController.SurfaceNormal;

        playerSimplePathMovement.SimpleGroundedHorizontalMovement(
            inputState.horizontalAxis, 
            movementProfile.groundedWalkVel,
            movementProfile.groundedWalkAccel,
            movementProfile.groundedFriction,
            Time.fixedDeltaTime, 
            groundNormal);
    }
}
