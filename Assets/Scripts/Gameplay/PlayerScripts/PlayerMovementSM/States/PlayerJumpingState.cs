using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class PlayerJumpingState : PlayerMovementState
{
    public PlayerJumpingState(StateMachine<PlayerBlackboard> stateMachine) : base(stateMachine)
    {
    }

    private float jumpTime;

    public override State<PlayerBlackboard> GetSwitchState()
    {
        if (movementContextController.IsGrounded)
        {
            return GetState<PlayerWalkingState>();
        }

        return GetState<PlayerFallingState>();
        
        return null;
    }
    
    public override void EnterState()
    {
        pathBody.pathVelocity.y += movementProfile.jumpStrength;
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        playerSimplePathMovement.SimpleHorizontalMovement(
            inputState.horizontalAxis, 
            movementProfile.airborneWalkVel,
            movementProfile.airborneWalkAccel,
            movementProfile.airborneFriction,
            Time.fixedDeltaTime, 
            Vector3.up);
    }

 
}
