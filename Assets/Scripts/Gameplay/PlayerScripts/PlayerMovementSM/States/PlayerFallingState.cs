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
        var transitions = GetTransitionTable<PlayerMovementTransitions>();
        return transitions.OnGroundedToWalk(ref c) || 
               transitions.OnInputToCoyoteTimeJump(ref c);
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
