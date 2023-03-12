using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagFootstoolJumpingState : ProtagState
{
    private int entryDirection;
    
    private bool TryFixedTransitionOut()
    {
        float jumpTime = stateMachine.CurrentStateFixedDuration + 0.00001f;

        bool minTimePassed = jumpTime > footstoolProfile.minJumpTime;
        bool maxTimePassed = jumpTime > footstoolProfile.jumpCurve.TimeDuration;

        bool isStableOnGround = controllerMotor.GroundingStatus.IsStableOnGround;
        bool forceOut = maxTimePassed || (isStableOnGround && minTimePassed);

        return forceOut && transitions.ToMovementSelector();
    }
    
    private bool TryTransitionOut()
    {
        float jumpTime = stateMachine.CurrentStateFixedDuration + 0.00001f;

        bool minTimePassed = jumpTime > footstoolProfile.minJumpTime;
        bool manualCancel = minTimePassed && !inputState.jumpHeld;

        return minTimePassed && transitions.ToCombatSelector()
               || manualCancel && transitions.ToMovementSelector();
    }

    public override void EnterState()
    {
        controllerAdapter.SetGravityEnabled(false);
        context.coyoteTimer = float.MaxValue;
        entryDirection = (int)Mathf.Sign(context.playerRotator.CurrentDir);
    }

    public override void ExitState()
    {
        controllerAdapter.pathVelocity.y = 
            Mathf.Min(controllerAdapter.pathVelocity.y, footstoolProfile.exitVelocity.y);
        
        controllerAdapter.pathVelocity.x = 
            entryDirection * Mathf.Min(entryDirection * controllerAdapter.pathVelocity.x, footstoolProfile.exitVelocity.x);

        controllerAdapter.SetGravityEnabled(true);
    }

    public override void UpdateState()
    {
        TryTransitionOut();
    }

    public override void FixedUpdateState()
    {
        Vector2 motionVel = footstoolProfile.jumpCurve.Differentiate(
            stateMachine.CurrentStateFixedDuration, 
            Time.fixedDeltaTime);

        controllerAdapter.pathVelocity.y = motionVel.y;
        controllerAdapter.pathVelocity.x = motionVel.x * entryDirection;

        TryFixedTransitionOut();
    }
}
