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

        bool isStableOnGround = controllerMotor.GroundingStatus.IsStableOnGround;
        bool forceOut = maxTimePassed || (isStableOnGround && minTimePassed);

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
        controllerAdapter.SetGravityEnabled(false);
        controllerMotor.ForceUnground(jumpProfile.minJumpTime);
        context.coyoteTimer = float.MaxValue;
    }

    public override void ExitState()
    {
        controllerAdapter.pathVelocity.y = Mathf.Min(controllerAdapter.pathVelocity.y, jumpProfile.jumpEndVel);
        controllerAdapter.SetGravityEnabled(true);
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

        controllerAdapter.pathVelocity.y = yVel;
        
        playerSimplePathMovement.SimpleAirborneHorizontalMovement(
            inputState.horizontalAxis, 
            hMoveProfile.airborneWalkVel,
            hMoveProfile.airborneWalkAccel,
            hMoveProfile.airborneFriction,
            Time.fixedDeltaTime,
            controllerAdapter.projectedNormal);

        TryFixedTransitionOut();
    }
}
