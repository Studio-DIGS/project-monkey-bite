using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplinePathPhysicsBody : MonoBehaviour
{
    [ColorHeader("Dependencies")]
    [SerializeField] private GameplayLevelStateSO currentLevelBlackboard;

    [ColorHeader("Config", ColorHeaderColor.Config)]
    [SerializeField] private float gravityAcceleration;
    
    // Fields
    [ReadOnly] public Vector2 velocity;
    [ReadOnly] public Vector2 bodyPosition;
    
    // Properties
    private SplineContainer LevelPath => currentLevelBlackboard.levelPath;

    private void OnEnable()
    {
        LockToPath();
    }

    private void FixedUpdate()
    {
        velocity.y -= Time.fixedDeltaTime * gravityAcceleration;
        
        bodyPosition += velocity * Time.fixedDeltaTime;
        float t = bodyPosition.x / LevelPath.CalculateLength();
        UpdatePositionOnPath(t);
    }

    /// <summary>
    /// Lock onto a path
    /// </summary>
    private void LockToPath()
    {
        float dist = SplineUtility.GetNearestPoint(
            LevelPath.Spline,
            transform.position, 
            out float3 nearestPos, 
            out float t);

        bodyPosition.x = dist;
    }

    private void UpdatePositionOnPath(float t)
    {
        Vector3 worldPos = LevelPath.EvaluatePosition(t);
        transform.position = new Vector3(
            worldPos.x,
            bodyPosition.y,
            worldPos.z
        );
    }
}
