using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagJumpingState : ProtagState
{
    public override bool TryTransition(ref State<ProtagBlackboard> c)
    {
        float jumpTime = stateMachine.CurrentStateDuration;
        
        bool minTimePassed = jumpTime > movementProfile.ftstlMinJumpTime;
        bool maxTimePassed = jumpTime > movementProfile.ftstlMaxJumpTime;

        bool isGrounded = movementContext.IsGrounded;
        bool tryManualCancel = (maxTimePassed || !inputState.jumpHeld);

        bool endJump = minTimePassed && (isGrounded || tryManualCancel);

        return (minTimePassed && combatTransitions.ToCombatSelector(ref c))
               || (endJump && moveTransitions.ToProtagStateSelector(ref c));
    }

    public override void EnterState()
    {
        pathBody.pathVelocity.y = movementProfile.jumpStrength;
        pathBody.SetGravityEnabled(false);
        context.coyoteTimer = float.MaxValue;
    }

    public override void ExitState()
    {
        pathBody.pathVelocity.y = Mathf.MoveTowards(
            pathBody.pathVelocity.y,
            0f,
            movementProfile.jumpEndVel);
        
        pathBody.SetGravityEnabled(true);
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        pathBody.pathVelocity.y = movementProfile.jumpStrength;
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
