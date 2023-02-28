using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagWalkingState : ProtagState
{
    public ProtagWalkingState(StateMachine<ProtagBlackboard> stateMachine) : base(stateMachine)
    {
        
    }


    public override bool TryTransition(ref State<ProtagBlackboard> c)
    {
        return transitions.DefaultGroundTransitions(ref c)
            || transitions.OnJumpPressedToJump(ref c)
            || transitions.WhenIdleToIdle(ref c);
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
        Vector2 groundNormal = movementContext.SurfaceNormal;

        playerSimplePathMovement.SimpleGroundedHorizontalMovement(
            inputState.horizontalAxis, 
            movementProfile.groundedWalkVel,
            movementProfile.groundedWalkAccel,
            movementProfile.groundedFriction,
            Time.fixedDeltaTime, 
            groundNormal);
    }
}
