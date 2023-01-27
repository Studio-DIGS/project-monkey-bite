using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class PlayerIdleState : PlayerMovementState
{
    public PlayerIdleState(StateMachine<PlayerBlackboard> stateMachine) : base(stateMachine)
    {
        blackboard.inputProvider.Events.OnJumpPressed += () => shouldJump = true;
    }

    private bool shouldJump;

    public override State<PlayerBlackboard> GetSwitchState()
    {
        if (shouldJump)
        {
            return GetState<PlayerJumpingState>();
        }
        if (!movementContextController.IsGrounded)
        {
            return GetState<PlayerFallingState>();
        }
        if (blackboard.inputState.horizontalAxis != 0)
        {
            return GetState<PlayerWalkingState>();
        }

        return null;
    }
    
    public override void EnterState()
    {
        shouldJump = false;
    }

    public override void ExitState()
    {
        pathBody.constrainVelocity = false;
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        playerSimplePathMovement.ApplyHorizontalFriction(
            movementProfile.groundedFriction,
            Time.fixedDeltaTime, 
            movementContextController.GroundedNormal);

        if(pathBody.pathVelocity.magnitude < 0.5f)
            pathBody.constrainVelocity = true;
    }
}
