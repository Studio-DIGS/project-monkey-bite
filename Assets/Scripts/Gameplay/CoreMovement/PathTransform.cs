using System;
using MushiCore.EditorAttributes;
using MushiCore.GizmoDrawer;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Splines;

/// <summary>
/// A component representing an objects position along the path
/// </summary>
public class PathTransform : MonoBehaviour
{
    private const float epsilon = 0.001f;
    
    [ColorHeader("Config")]
    [SerializeField] private Vector2 position;
    [SerializeField] private SplineContainer currentPath;
    [SerializeField] private bool selfInitialize;
    [SerializeField] public bool autoSyncTransform = false;

    public Vector2 Position
    {
        set => SetPosition(value);
        get => position;
    }

    public Vector3 WCurrentTangent => wCurrentTangent;
    public Vector3 WCurrentUp => wCurrentUp;
    public Vector3 WCurrentNormal => wCurrentNormal;
    public Vector3 WPos => wPos;
    
    private float currentPathLength;
    private float normalizedX;
    private Vector3 wCurrentTangent;
    private Vector3 wCurrentUp;
    private Vector3 wCurrentNormal;
    private Vector3 wPos;

    private bool loop;

    private void OnEnable()
    {
        if(selfInitialize)
            Initialize(currentPath, transform.position);
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if(currentPath)
            Position = position;
#endif
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
        
        wCurrentTangent = currentPath.EvaluateTangent(normalizedX);
        wCurrentTangent.Normalize();
        wCurrentUp = Vector3.up;
        wCurrentNormal = Vector3.Cross(wCurrentTangent, wCurrentUp);
        wCurrentNormal.Normalize();
        
        wPos = EvaluatePos(value);
        
        if(autoSyncTransform)
            SyncWorldPosition();
    }

    /// <summary>
    /// Snap to nearest path position
    /// </summary>
    /// <param name="point"></param>
    public void SnapToNearestPoint(Vector3 point)
    {
        GetNearestPoint(point, out Vector3 wNearestPos, out Vector2 sNearestPos);
        SetPosition(sNearestPos);
    }

    public void SyncWorldPosition()
    {
        transform.position = wPos;
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
    
    public GizmoDrawerObject[] GetGizmoLine(Color c,Vector2 sP1, Vector2 sP2)
    {
        var sp2 = EvaluatePos(sP2);
        return new GizmoDrawerObject[]
        {
            new GizmoDrawerLine(c, EvaluatePos(sP1), sp2),
            new GizmoDrawerWireSphere(c, sp2, 0.025f)
        };
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        var pos = transform.position + Vector3.up * 0.1f;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(pos, pos + wCurrentTangent);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pos, pos + wCurrentUp);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pos, pos + wCurrentNormal);
    }
#endif
    
}
