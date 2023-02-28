using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagFallingState : ProtagState
{
    public ProtagFallingState(StateMachine<ProtagBlackboard> stateMachine) : base(stateMachine)
    {
        
    }


    public override bool TryTransition(ref State<ProtagBlackboard> c)
    {
        return transitions.WhenGroundedToWalk(ref c)
            || transitions.OnJumpPressedToFootstoolJump(ref c)
            || transitions.OnJumpPressedToJump(ref c);
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        blackboard.coyoteTimer += Time.deltaTime;
    }

    public override void FixedUpdateState()
    {
        if (!movementContext.IsOnSurface)
        {
            playerSimplePathMovement.SimpleAirborneHorizontalMovement(
                inputState.horizontalAxis,
                movementProfile.airborneWalkVel,
                movementProfile.airborneWalkAccel,
                movementProfile.airborneFriction,
                Time.fixedDeltaTime,
                movementContext.SurfaceNormal);
        }
    }

    
}
