using UnityEngine;

/// <summary>
/// A state machine used for evaluating a motion curve over time, keeping track of addition information like velocity and velocity steps
/// </summary>
public class MotionCurveEvaluator
{
    /// <summary>
    /// The current position evaluated from the motion curve
    /// </summary>
    public float CurrentPosition => currentPosition;
    /// <summary>
    /// The current velocity evaluated from the previous step
    /// </summary>
    public float CurrentVel => currentVel;
    /// <summary>
    /// The current velocity difference evaluated from the previous two velocities
    /// </summary>
    public float CurrentVelStep => currentVelStep;
    
    // Motion curve values
    private AnimationCurve sourceCurve;
    private float totalDuration;
    private float amplitude;

    // Time
    private float currentTimeRaw;
    
    // State
    private float previousPosition;
    private float currentPosition;
    
    private float previousVel;
    private float currentVel;
    
    private float currentVelStep;

    public MotionCurveEvaluator(AnimationCurve sourceCurve, float amplitude, float totalDuration)
    {
        this.sourceCurve = sourceCurve;
        this.amplitude = amplitude;
        this.totalDuration = totalDuration;
    }

    /// <summary>
    /// Move the motion curve evaluator forwards by some deltaTime. For consistent results use a constant deltaTime (e.g fixedDeltaTime)
    /// </summary>
    /// <param name="deltaTime"></param>
    
    public void StepForward(float deltaTime)
    {
        currentTimeRaw += deltaTime;
        currentTimeRaw = Mathf.Clamp(currentTimeRaw, 0f, totalDuration);
        EvaluateAll(deltaTime);
    }

    /// <summary>
    /// Recalculate all the properties related to the current position on the motion curve
    /// </summary>
    /// <param name="deltaTime"></param>
    private void EvaluateAll(float deltaTime)
    {
        float normalizedT = currentTimeRaw / totalDuration;

        previousPosition = currentPosition;
        
        currentPosition = sourceCurve.Evaluate(normalizedT) * amplitude;

        previousVel = currentVel;
        currentVel = (currentPosition - previousPosition) / deltaTime;

        currentVelStep = currentVel - previousVel;
    }
}
