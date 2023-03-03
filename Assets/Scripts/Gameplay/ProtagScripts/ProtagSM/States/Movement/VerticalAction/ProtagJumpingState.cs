using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagJumpingState : ProtagState
{
    public override bool TryTransition(ref State<ProtagBlackboard> c)
    {
        float jumpTime = stateMachine.CurrentStateFixedDuration + 0.00001f;

        bool minTimePassed = jumpTime > jumpProfile.minJumpTime;
        bool maxTimePassed = jumpTime > jumpProfile.jumpCurve.TimeDuration;

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
        float yVel = jumpProfile.jumpCurve.DifferentiateY(
            stateMachine.CurrentStateFixedDuration, 
            Time.fixedDeltaTime);

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
