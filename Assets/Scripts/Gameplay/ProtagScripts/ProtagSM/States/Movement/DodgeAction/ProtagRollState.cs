using SimpleStateMachine;
using UnityEngine;

public class ProtagRollState : ProtagState
{
    private MotionCurve rollCurve;
    private float entryDirection;
    
    private bool TryFixedTransitionOut()
    {
        bool durationFinished = stateMachine.CurrentStateDuration >= rollCurve.TimeDuration;
        return durationFinished && transitions.ToProtagStateSelector();
    }

    public override void EnterState()
    {
        Debug.Log("ROLL");
        rollCurve = rollProfile.rollMotionCurve;
        entryDirection = context.playerRotator.CurrentDir;
        animationController.Play("rollAnim");
    }

    public override void ExitState()
    {
        controllerMotor.pathVelocity.y = 
            Mathf.Min(controllerMotor.pathVelocity.y, rollProfile.exitVelocity.y);

        controllerMotor.pathVelocity.x =
            entryDirection * rollProfile.exitVelocity.x;

    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        var motionVel = rollCurve.Differentiate(
            stateMachine.CurrentStateFixedDuration,
            Time.fixedDeltaTime);
        
        controllerMotor.pathVelocity.y = motionVel.y;
        controllerMotor.pathVelocity.x = motionVel.x * entryDirection;

        TryFixedTransitionOut();
    }
}
