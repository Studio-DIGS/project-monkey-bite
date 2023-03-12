using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Splines;

public class PathTransform : MonoBehaviour
{
    private const float epsilon = 0.001f;
    
    [SerializeField] private Vector2 position;
    [SerializeField] private SplineContainer currentPath;
    [SerializeField] private bool selfInitialize;

    public Vector2 Position
    {
        set => SetPosition(value);
        get => position;
    }

    public Vector3 CTangent => cTangent;
    public Vector3 CUp => cUp;
    public Vector3 CNormal => cNormal;
    public Vector3 WorldPos => worldPos;
    
    private float currentPathLength;
    private float normalizedX;
    private Vector3 cTangent;
    private Vector3 cUp;
    private Vector3 cNormal;
    private Vector3 worldPos;

    private bool loop;

    private void OnEnable()
    {
        if(selfInitialize)
            Initialize(currentPath, transform.position);
    }

    private void OnValidate()
    {
        if(currentPath)
            Position = position;
    }

    public void Initialize(SplineContainer path, Vector3 worldPosition)
    {
        currentPath = path;
        currentPathLength = currentPath.CalculateLength();
        SnapToNearestPoint(worldPosition);
        loop = path.Spline.Closed;
    }
    
    public void SetPosition(Vector2 value, bool updateTransform = true)
    {
        // Clamping
        if (loop)
        {
            value.x = Mathf.Repeat(value.x, currentPathLength);
        }
        else
        {
            value.x = Mathf.Clamp(value.x, epsilon, currentPathLength - epsilon);
        }
        
        position = value;
        
        normalizedX = value.x / currentPathLength;
        
        cTangent = currentPath.EvaluateTangent(normalizedX);
        cTangent.Normalize();
        cUp = currentPath.EvaluateUpVector(normalizedX);
        cUp.Normalize();
        cNormal = Vector3.Cross(cTangent, cUp);
        cNormal.Normalize();
        
        worldPos = EvaluatePos(value);
        
        if(updateTransform)
            transform.position = worldPos;
    }

    public void SnapToNearestPoint(Vector3 point, bool updateTransform = true)
    {
        GetNearestPoint(point, out Vector3 nearestPos, out float t);
        var pathPos = new Vector2(t * currentPathLength, point.y);
        SetPosition(pathPos, updateTransform);
    }

    public void GetNearestPoint(Vector3 point, out Vector3 nearestPos, out float t)
    {
        SplineUtility.GetNearestPoint(
            currentPath.Spline,
            point, 
            out float3 nearestPosFloat3, 
            out float outT,
            resolution: 8,
            iterations: 4);
        nearestPos = (Vector3)nearestPosFloat3;
        t = outT;
    }

    public Vector3 EvaluatePos(Vector2 evalPos)
    {
        normalizedX = evalPos.x / currentPathLength;
        var result = currentPath.EvaluatePosition(normalizedX);
        result.y = evalPos.y;
        return result;
    }

    public Vector2 ProjectVectorOntoPlane(Vector3 vector)
    {
        float cachedY = vector.y;
        vector.y = 0f;
        Vector2 projectedHorizontal = Vector3.Dot(vector, cTangent) * Vector2.right;
        projectedHorizontal.y = cachedY;
        return projectedHorizontal;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        var pos = transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(pos, pos + cTangent);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pos, pos + cUp);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pos, pos + cNormal);
    }
#endif
    
}
