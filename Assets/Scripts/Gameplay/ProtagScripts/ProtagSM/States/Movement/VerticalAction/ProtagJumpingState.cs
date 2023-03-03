using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagJumpingState : ProtagState
{
    public override bool TryTransition(ref State<ProtagBlackboard> c)
    {
        float jumpTime = stateMachine.CurrentStateDuration;

        bool minTimePassed = jumpTime > jumpProfile.minJumpTime;
        bool maxTimePassed = jumpTime > jumpProfile.maxJumpTime;

        bool isGrounded = movementContext.IsGrounded;
        bool tryManualCancel = (maxTimePassed || !inputState.jumpHeld);

        bool endJump = minTimePassed && (isGrounded || tryManualCancel);

        return (minTimePassed && combatTransitions.ToCombatSelector(ref c))
               || (endJump && moveTransitions.ToProtagStateSelector(ref c));
    }

    public override void EnterState()
    {
        pathBody.SetGravityEnabled(false);
        context.coyoteTimer = float.MaxValue;
    }

    public override void ExitState()
    {
        pathBody.pathVelocity.y = Mathf.Min(pathBody.pathVelocity.y, jumpProfile.jumpEndVel);
        
        pathBody.SetGravityEnabled(true);
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        float yVel = jumpProfile.jumpCurve.SampleSlopeTime(
                         stateMachine.CurrentStateFixedDuration,
                         Time.fixedDeltaTime,
                         jumpProfile.maxJumpTime) 
                     * jumpProfile.jumpHeight;

        pathBody.pathVelocity.y = yVel;
        
        if (!movementContext.IsOnSurface)
        {
            playerSimplePathMovement.SimpleAirborneHorizontalMovement(
                inputState.horizontalAxis, 
                hMoveProfile.airborneWalkVel,
                hMoveProfile.airborneWalkAccel,
                hMoveProfile.airborneFriction,
                Time.fixedDeltaTime, 
                movementContext.SurfaceNormal);
        }
    }

 
}
