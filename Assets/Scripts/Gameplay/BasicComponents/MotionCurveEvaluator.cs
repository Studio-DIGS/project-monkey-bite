using UnityEngine;

public class MotionCurveEvaluator
{
    public float CurrentPosition => currentPosition;
    public float CurrentVel => currentVel;
    public float CurrentVelStep => currentVelStep;
    
    // Motion curve representation
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

    public void StepForward(float deltaTime)
    {
        currentTimeRaw += deltaTime;
        currentTimeRaw = Mathf.Clamp(currentTimeRaw, 0f, totalDuration);
        EvaluateAll(deltaTime);
    }

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
