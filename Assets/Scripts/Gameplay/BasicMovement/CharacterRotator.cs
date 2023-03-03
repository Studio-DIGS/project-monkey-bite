using System;
using MushiCore.EditorAttributes;
using UnityEngine;

/// <summary>
/// Deals with aligning and rotating to match direction/path
/// </summary>
public class CharacterRotator : MonoBehaviour
{
    [ColorHeader("Dependencies")]
    [SerializeField] private SplinePathPhysicsBody splineBody;
    [SerializeField] private Transform[] targetTransforms;

    private int currentDir;

    public int CurrentDir => currentDir;
    
    /// <summary>
    /// Align the target transform to path
    /// </summary>
    /// <param name="dir"></param>
    public void AlignDirection(float dir)
    {
        if (dir == 0) return;
        currentDir = (int)Mathf.Sign(dir);
        foreach(var target in targetTransforms)
            target.rotation = Quaternion.LookRotation(dir * splineBody.GetCurrentTangent(), Vector3.up);
    }
}
