using SimpleStateMachine;
using UnityEngine;

public class ProtagRollState : ProtagState
{
    private MotionCurve rollCurve;
    private float entryDirection;
    
    private bool TryTransitionOut()
    {
        bool durationFinished = stateMachine.CurrentStateDuration >= rollCurve.TimeDuration;
        return durationFinished && transitions.ToProtagStateSelector();
    }

    public override void EnterState()
    {
        Debug.Log("ROLL");
        rollCurve = rollProfile.rollMotionCurve;
        entryDirection = context.playerRotator.CurrentDir;
    }

    public override void ExitState()
    {
        pathBody.pathVelocity.y = 
            Mathf.Min(pathBody.pathVelocity.y, rollProfile.exitVelocity.y);

        pathBody.pathVelocity.x =
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
        
        pathBody.pathVelocity.y = motionVel.y;
        pathBody.pathVelocity.x = motionVel.x * entryDirection;

        TryTransitionOut();
    }
}
