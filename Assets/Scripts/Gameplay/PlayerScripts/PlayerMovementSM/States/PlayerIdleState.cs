using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class PlayerIdleState : PlayerMovementState
{
    public PlayerIdleState(StateMachine<PlayerBlackboard> stateMachine) : base(stateMachine)
    {
        
    }

    public override bool TryTransition(ref State<PlayerBlackboard> c)
    {
        return GetTransitionTable<PlayerMovementTransitions>().DefaultGroundTransitions(ref c);
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
            movementContextController.SurfaceNormal);

        if(pathBody.pathVelocity.magnitude < 0.5f)
            pathBody.constrainVelocity = true;
    }
}
