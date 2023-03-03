using MushiCore.EditorAttributes;
using UnityEngine;

[System.Serializable]
public class MotionCurve
{
    private const float delta = 0.00001f;
    
    [ColorHeader("X Component")]
    [SerializeField] private AnimationCurve xCurve;
    [SerializeField] float xMagnitude;
    
    [ColorHeader("Y Component")]
    [SerializeField] private AnimationCurve yCurve;
    [SerializeField]private float yMagnitude;

    [ColorHeader("Config", ColorHeaderColor.Config)]
    [SerializeField] private float timeDuration;

    public float TimeDuration => timeDuration;

    public float EvaluateX(float tUNorm)
    {
        return xCurve.Evaluate(tUNorm / timeDuration) * xMagnitude;
    }

    public float EvaluateY(float tUNorm)
    {
        return yCurve.Evaluate(tUNorm / timeDuration) * yMagnitude;
    }

    public Vector2 Differentiate(float tUNorm, float timeDelta = delta)
    {
        return new Vector2(DifferentiateX(tUNorm, timeDelta), DifferentiateY(tUNorm, timeDelta));
    }
    
    public float DifferentiateX(float tUNorm, float timeDelta = delta)
    {
        return DifferentiateInternal(xCurve, tUNorm, timeDelta) * xMagnitude;
    }

    public float DifferentiateY(float tUNorm, float timeDelta = delta)
    {
        return DifferentiateInternal(yCurve, tUNorm, timeDelta) * yMagnitude;
    }
    
    private float DifferentiateInternal(AnimationCurve curve, float tUNorm, float timeDelta)
    {
        float normalizedTime = tUNorm / timeDuration;
        float normalizedInterval = timeDelta / timeDuration;

        normalizedTime = Mathf.Clamp(normalizedTime, normalizedInterval, 1 - normalizedInterval);
        
        float diff = curve.Evaluate(normalizedTime + normalizedInterval)
                     - curve.Evaluate(normalizedTime);
        
        return diff / timeDelta;
    }
}
