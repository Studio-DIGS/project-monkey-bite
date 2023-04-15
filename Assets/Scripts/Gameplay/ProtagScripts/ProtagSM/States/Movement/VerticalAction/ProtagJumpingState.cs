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
        
        controllerMotor.SetForceUnground(!minTimePassed);

        bool isStableOnGround = controllerMotor.CurrentGroundState.IsStableOnGround;
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
        controllerMotor.SetGravityEnabled(false);
        controllerMotor.SetForceUnground(true);
        controllerMotor.pathVelocity.y = 0f;
        context.coyoteTimer = float.MaxValue;
        prevYVel = 0f;
    }

    public override void ExitState()
    {
        controllerMotor.pathVelocity.y = Mathf.Min(controllerMotor.pathVelocity.y, jumpProfile.jumpEndVel);
        controllerMotor.SetGravityEnabled(true);
        controllerMotor.SetForceUnground(false);
    }

    public override void UpdateState(float deltaTime)
    {
        TryTransitionOut();
    }

    private float prevYVel;

    public override void FixedUpdateState(float fixedDeltaTime)
    {
        float yVel = jumpProfile.jumpCurve.DifferentiateY(
            stateMachine.CurrentStateFixedDuration, 
            fixedDeltaTime);

        controllerMotor.pathVelocity.y += yVel - prevYVel;

        prevYVel = yVel;
        
        playerSimplePathMovement.SimpleAirborneHorizontalMovement(
            inputState.horizontalAxis, 
            hMoveProfile.airborneWalkVel,
            hMoveProfile.airborneWalkAccel,
            hMoveProfile.airborneFriction,
            fixedDeltaTime,
            controllerMotor.CurrentGroundState.GroundNormal);

        TryFixedTransitionOut();
    }
}
