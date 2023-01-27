using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class PlayerFallingState : PlayerMovementState
{
    public PlayerFallingState(StateMachine<PlayerBlackboard> stateMachine) : base(stateMachine)
    {
    }

    public override State<PlayerBlackboard> GetSwitchState()
    {
        if (movementContextController.IsGrounded)
        {
            return GetState<PlayerWalkingState>();
        }
        
        return null;
    }
    
    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        
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
