using UnityEngine;
using UnityEngine.UIElements;

public static class GameplayExtensionMethods 
{
    /// <summary>
    /// Calculate the slope using a sample time interval
    /// </summary>
    /// <param name="curve"></param>
    /// <param name="leftTime">time point to sample from(left side of interval)</param>
    /// <param name="timeInterval">width of interval</param>
    /// <param name="scale">time scale</param>
    /// <returns></returns>
    public static float SampleSlopeTime(
        this AnimationCurve curve,
        float leftTime, 
        float timeInterval,
        float scale = 1)
    {
        float normalizedTime = leftTime / scale;
        float normalizedInterval = timeInterval / scale;
        
        float delta = curve.Evaluate(normalizedTime + normalizedInterval)
                     - curve.Evaluate(normalizedTime);
        
        return delta / timeInterval;
    }
}
