using MushiCore.EditorAttributes;
using UnityEngine;

/// <summary>
/// A motion curve represents position motion over time. Intended to be used with <see cref="MotionCurveEvaluator"/>
/// </summary>
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

    public Vector2 Evaluate(float tRaw)
    {
        float tNormalized = tRaw / timeDuration;

        return new Vector2(
            xCurve.Evaluate(tNormalized),
            yCurve.Evaluate(tNormalized));
    }

    public MotionCurveEvaluator GetXEvaluator()
    {
        return new MotionCurveEvaluator(xCurve, xMagnitude, timeDuration);
    }
    
    public MotionCurveEvaluator GetYEvaluator()
    {
        return new MotionCurveEvaluator(yCurve, yMagnitude, timeDuration);
    }
}
