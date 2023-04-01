using System;
using MushiCore.GizmoDrawer;
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
    [SerializeField] public bool autoSyncTransform = false;

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
        SyncWorldPosition();
        loop = path.Spline.Closed;
    }
    
    private void SetPosition(Vector2 value)
    {
        // Clamping
        value.x = LoopX(value.x);
        
        position = value;
        
        normalizedX = value.x / currentPathLength;
        
        cTangent = currentPath.EvaluateTangent(normalizedX);
        cTangent.Normalize();
        cUp = Vector3.up;
        cNormal = Vector3.Cross(cTangent, cUp);
        cNormal.Normalize();
        
        worldPos = EvaluatePos(value);
        
        if(autoSyncTransform)
            SyncWorldPosition();
    }

    public void SnapToNearestPoint(Vector3 point)
    {
        GetNearestPoint(point, out Vector3 wNearestPos, out Vector2 sNearestPos);
        SetPosition(sNearestPos);
    }

    public void SyncWorldPosition()
    {
        transform.position = worldPos;
    }

    public void GetNearestPoint(Vector3 point, out Vector3 wNearestPos, out Vector2 sNearestPos)
    {
        Vector3 testPoint = point;
        testPoint.y = 0;
        SplineUtility.GetNearestPoint(
            currentPath.Spline,
            testPoint, 
            out float3 nearestPosFloat3, 
            out float outT,
            resolution: 8,
            iterations: 4);
        wNearestPos = (Vector3)nearestPosFloat3;
        wNearestPos.y = point.y;
        sNearestPos = new Vector2(outT * currentPathLength, point.y);
    }

    public Vector3 EvaluatePos(Vector2 evalPos)
    {
        normalizedX = evalPos.x / currentPathLength;
        var result = currentPath.EvaluatePosition(normalizedX);
        result.y = evalPos.y;
        return result;
    }

    public Vector3 EvaluateTangent(Vector2 evalPos)
    {
        var tangent = currentPath.EvaluateTangent(evalPos.x / currentPathLength);
        tangent.y = 0;
        return tangent;
    }

    public Vector2 ProjectToPathSpace(Vector3 vector)
    {
        float cachedY = vector.y;
        vector.y = 0f;
        Vector2 projectedHorizontal = Vector3.Dot(vector, cTangent) * Vector2.right;
        projectedHorizontal.y = cachedY;
        return projectedHorizontal;
    }
    
    public Vector2 ProjectToPathSpace(Vector3 vector, Vector2 sEvaluatePos)
    {
        float t = sEvaluatePos.x / currentPathLength;
        float cachedY = vector.y;
        vector.y = 0f;
        Vector2 projectedHorizontal = Vector3.Dot(vector, currentPath.EvaluateTangent(t)) * Vector2.right;
        projectedHorizontal.y = cachedY;
        return projectedHorizontal;
    }

    public Vector3 ProjectVectorFromPlane(Vector2 planeVector)
    {
        float cachedY = planeVector.y;
        Vector3 res = cTangent * planeVector.x;
        res.y = cachedY;
        return res;
    }

    public GizmoDrawerLine GetGizmoLine(Color c,Vector2 sP1, Vector2 sP2)
    {
        return new GizmoDrawerLine(c, EvaluatePos(sP1), EvaluatePos(sP2));
    }

    public float LoopX(float sPosX)
    {
        if (loop)
        {
            return Mathf.Repeat(sPosX, currentPathLength);
        }
        else
        {
            return Mathf.Clamp(sPosX, epsilon, currentPathLength - epsilon);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        var pos = transform.position + Vector3.up * 0.1f;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(pos, pos + cTangent);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pos, pos + cUp);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pos, pos + cNormal);
    }
#endif
    
}
