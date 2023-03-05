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

        bool isGrounded = movementContext.IsGrounded;
        bool forceOut = maxTimePassed || (isGrounded && minTimePassed);

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
        pathBody.SetGravityEnabled(false);
        context.coyoteTimer = float.MaxValue;
        entryDirection = (int)Mathf.Sign(context.playerRotator.CurrentDir);
    }

    public override void ExitState()
    {
        pathBody.pathVelocity.y = 
            Mathf.Min(pathBody.pathVelocity.y, footstoolProfile.exitVelocity.y);
        
        pathBody.pathVelocity.x = 
            entryDirection * Mathf.Min(entryDirection * pathBody.pathVelocity.x, footstoolProfile.exitVelocity.x);

        pathBody.SetGravityEnabled(true);
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

        pathBody.pathVelocity.y = motionVel.y;
        pathBody.pathVelocity.x = motionVel.x * entryDirection;

        TryFixedTransitionOut();
    }
}
