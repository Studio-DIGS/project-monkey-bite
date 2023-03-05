using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagJumpingState : ProtagState
{
    /*
     *In order to have consistent timing, having the jump time out should be checked in fixed update
     */
    private bool TryFixedTransitionOut()
    {
        float jumpTime = stateMachine.CurrentStateFixedDuration + 0.00001f;

        bool minTimePassed = jumpTime > jumpProfile.minJumpTime;
        bool maxTimePassed = jumpTime > jumpProfile.jumpCurve.TimeDuration;

        bool isGrounded = movementContext.IsGrounded;
        bool forceOut = maxTimePassed || (isGrounded && minTimePassed);

        return forceOut && transitions.ToMovementSelector();
    }

    /*
     * Input based exits should be checked in update in order to correctly get input on time
     */
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
        
        playerSimplePathMovement.SimpleAirborneHorizontalMovement(
            inputState.horizontalAxis, 
            hMoveProfile.airborneWalkVel,
            hMoveProfile.airborneWalkAccel,
            hMoveProfile.airborneFriction,
            Time.fixedDeltaTime, 
            movementContext.IsOnSurface,
            movementContext.SurfaceNormal);

        TryFixedTransitionOut();
    }
}
