using UnityEngine;
using UnityEngine.UIElements;

public static class GameplayExtensionMethods 
{
    /// <summary>
    /// Calculate the slope using a sample time interval
    /// </summary>
    /// <param name="curve"></param>
    /// <param name="time">time point to sample from(left side of interval)</param>
    /// <param name="timeInterval">width of interval</param>
    /// <param name="timeScale">time scale</param>
    /// <returns></returns>
    public static float Differentiate(
        this AnimationCurve curve,
        float time, 
        float timeInterval,
        float timeScale = 1)
    {
        float normalizedTime = time / timeScale;
        float normalizedInterval = timeInterval / timeScale;
        
        float diff = curve.Evaluate(normalizedTime + normalizedInterval)
                     - curve.Evaluate(normalizedTime);
        
        return diff / timeInterval;
    }

    public static Vector2 ProjectOntoPlane(this Vector2 toProject, Vector2 normal)
    {
        Vector2 proj = Vector2.Dot(toProject, normal) * normal;
        return toProject - proj;
    }
}
