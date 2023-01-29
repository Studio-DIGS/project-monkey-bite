using System;
using UnityEngine;

/// <summary>
/// Deals with aligning and rotating to match direction/path
/// </summary>
public class CharacterRotator : MonoBehaviour
{
    [ColorHeader("Dependencies")]
    [SerializeField] private SplinePathPhysicsBody splineBody;
    [SerializeField] private Transform[] targetTransforms;


    /// <summary>
    /// Align the target transform to path
    /// </summary>
    /// <param name="dir"></param>
    public void AlignDirection(float dir)
    {
        if (dir == 0) return;
        foreach(var target in targetTransforms)
            target.rotation = Quaternion.LookRotation(dir * splineBody.GetCurrentTangent(), Vector3.up);
    }
}
