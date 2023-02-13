using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class PlayerFallingState : PlayerMovementState
{
    public PlayerFallingState(StateMachine<PlayerBlackboard> stateMachine) : base(stateMachine)
    {
        
    }


    public override bool TryTransition(ref State<PlayerBlackboard> c)
    {
        return transitions.WhenGroundedToWalk(ref c);
    }

    public override void EnterState()
    {
        transitions.AddOnJumpPressedToJump();
        transitions.AddOnJumpPressedToFootstoolJump();
    }

    public override void ExitState()
    {
        transitions.RemoveOnJumpPressedToJump();
        transitions.RemoveOnJumpPressedToFootstoolJump();
    }

    public override void UpdateState()
    {
        blackboard.coyoteTimer += Time.deltaTime;
    }

    public override void FixedUpdateState()
    {
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
