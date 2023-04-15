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

        controllerMotor.pathVelocity = Vector2.zero;
        
        xMotionEvaluator = rollCurve.GetXEvaluator();
        yMotionEvaluator = rollCurve.GetYEvaluator();
    }

    public override void ExitState()
    {
        controllerMotor.pathVelocity.y = 
            Mathf.Min(controllerMotor.pathVelocity.y, rollProfile.exitVelocity.y);

        controllerMotor.pathVelocity.x =
            entryDirection * rollProfile.exitVelocity.x;
    }

    public override void UpdateState(float deltaTime)
    {
        
    }
    
    private MotionCurveEvaluator xMotionEvaluator;
    private MotionCurveEvaluator yMotionEvaluator;

    public override void FixedUpdateState(float fixedDeltaTime)
    {
        xMotionEvaluator.StepForward(fixedDeltaTime);
        yMotionEvaluator.StepForward(fixedDeltaTime);

        controllerMotor.pathVelocity.x = xMotionEvaluator.CurrentVel * entryDirection;
        controllerMotor.pathVelocity.y += yMotionEvaluator.CurrentVelStep;

        TryFixedTransitionOut();
    }
}
