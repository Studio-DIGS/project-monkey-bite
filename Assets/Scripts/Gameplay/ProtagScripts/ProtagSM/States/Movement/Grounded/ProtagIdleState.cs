using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagIdleState : ProtagState
{
    public ProtagIdleState(StateMachine<ProtagBlackboard> stateMachine) : base(stateMachine)
    {
        
    }

    public override bool TryTransition(ref State<ProtagBlackboard> c)
    {
        return transitions.WhenAirborneToFalling(ref c)
            || transitions.OnJumpPressedToJump(ref c)
            || transitions.WhenWalkingToWalking(ref c);
    }

    public override void EnterState()
    {
        blackboard.coyoteTimer = 0f;
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
        pathBody.pathVelocity += playerSimplePathMovement.CalculateHorizontalFrictionStep(
            movementProfile.groundedFriction,
            Time.fixedDeltaTime, 
            movementContext.SurfaceNormal);

        if(pathBody.pathVelocity.magnitude < 0.5f)
            pathBody.constrainVelocity = true;
    }
}
