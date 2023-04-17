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
        context.coyoteTimer = float.MaxValue;

        controllerMotor.pathVelocity.y = 0f;
        motionEvaluator = jumpProfile.jumpCurve.GetYEvaluator();
        
        animationController.Play("Fall");
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

    private MotionCurveEvaluator motionEvaluator;

    public override void FixedUpdateState(float fixedDeltaTime)
    {
        motionEvaluator.StepForward(fixedDeltaTime);
        
        controllerMotor.pathVelocity.y += motionEvaluator.CurrentVelStep;

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
