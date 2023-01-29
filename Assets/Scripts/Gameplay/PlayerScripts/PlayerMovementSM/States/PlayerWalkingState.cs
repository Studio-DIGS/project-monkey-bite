using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class PlayerWalkingState : PlayerMovementState
{
    public PlayerWalkingState(StateMachine<PlayerBlackboard> stateMachine) : base(stateMachine)
    {
        
    }


    public override bool TryTransition(ref State<PlayerBlackboard> c)
    {
        return transitions.DefaultGroundTransitions(ref c);
    }

    public override void EnterState()
    {
        WalkMovement();
        blackboard.coyoteTimer = 0f;
        transitions.AddOnJumpPressedToJump();
    }

    public override void ExitState()
    {
        transitions.RemoveOnJumpPressedToJump();
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
