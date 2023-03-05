using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagJumpingState : ProtagState
{
    private bool TryFixedTransitionOut()
    {
        float jumpTime = stateMachine.CurrentStateFixedDuration + 0.00001f;

        bool minTimePassed = jumpTime > jumpProfile.minJumpTime;
        bool maxTimePassed = jumpTime > jumpProfile.jumpCurve.TimeDuration;

        bool isGrounded = movementContext.IsGrounded;
        bool forceOut = maxTimePassed || (isGrounded && minTimePassed);

        return forceOut && transitions.ToMovementSelector();
    }

    private bool TryTransitionOut()
    {
        float jumpTime = stateMachine.CurrentStateFixedDuration + 0.00001f;

        bool minTimePassed = jumpTime > jumpProfile.minJumpTime;
        bool manualCancel = minTimePassed && !inputState.jumpHeld;

        return minTimePassed && transitions.ToCombatSelector()
               || manualCancel && transitions.ToMovementSelector();
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
        TryTransitionOut();
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
                movementContext.IsOnSurface,
                movementContext.SurfaceNormal);
        }

        TryFixedTransitionOut();
    }
}
