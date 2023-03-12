using System;
using MushiCore.EditorAttributes;
using UnityEngine;

/// <summary>
/// Deals with aligning and rotating to match direction/path
/// </summary>
public class CharacterRotator : MonoBehaviour
{
    [ColorHeader("Dependencies")]
    [SerializeField] private Transform[] targetTransforms;

    private int currentDir = 1;
    private PathTransform pathTransform;

    public int CurrentDir => currentDir;
    
    public void Initialize(PathTransform pathTransform)
    {
        this.pathTransform = pathTransform;
    }
    
    /// <summary>
    /// Align the target transform to path
    /// </summary>
    /// <param name="dir"></param>
    public void AlignDirection(float dir)
    {
        if (dir != 0)
            currentDir = (int)Mathf.Sign(dir);
            
        foreach(var target in targetTransforms)
            target.rotation = Quaternion.LookRotation(currentDir * pathTransform.CNormal, pathTransform.CUp);
    }
}
