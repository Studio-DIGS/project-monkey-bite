using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// Simple physics/collider body for spline paths. Does not support custom Up direction
/// Also doesn't support incoming collisions - cuz shits too hard man
/// </summary>
public class SplinePathPhysicsBody : MonoBehaviour
{
    [ColorHeader("Dependencies")]
    [SerializeField] private GameplayLevelStateSO currentLevelBlackboard;
    [SerializeField] private CapsuleCollider collider;

    [ColorHeader("Config", ColorHeaderColor.Config)]
    [SerializeField] private float gravityAcceleration;
    [SerializeField] private float collisionResolutionOffset;
    [SerializeField] private LayerMask collisionMask;
    
    // Fields
    [ColorHeader("Physics State")]
    public Vector2 pathVelocity;
    public Vector2 pathPosition;

    private float pathLength;
    
    // Properties
    private SplineContainer SplinePath => currentLevelBlackboard.levelPath;

    // Gizmo debugging fields
    private Vector3 collisionResolutionVel;
    private Vector3 splDir;

    private void OnEnable()
    {
        LockToPath();
        pathPosition.y = transform.position.y;
        pathLength = SplinePath.CalculateLength();
    }
    
    /// <summary>
    /// Lock onto a path
    /// </summary>
    private void LockToPath()
    {
        float dist = SplineUtility.GetNearestPoint(
            SplinePath.Spline,
            transform.position, 
            out float3 nearestPos, 
            out float t);

        pathPosition.x = dist;
    }

    private void FixedUpdate()
    {
        ApplyForces();
        ResolveCollisions();
        ApplyVelocity();
    }

    private void ApplyForces()
    {
        pathVelocity.y -= Time.fixedDeltaTime * gravityAcceleration;
    }

    private void ResolveCollisions()
    {
        // Update position so we get correct collider bounds when sweeping
        float t = pathPosition.x / pathLength;
        UpdatePositionOnPath(t);
        
        // Do some conversions to get velocity step for this fixedUpdate
        Vector3 worldVel = PathToWorldVec(pathVelocity, t);
        Vector3 sweepDir = worldVel.normalized;
        float dist = worldVel.magnitude * Time.fixedDeltaTime;
        
        // Get bounds of the collider
        Vector3 center = collider.bounds.center;
        Vector3 bounds = collider.transform.up * (collider.height/2f - collider.radius);
        
        // Perform sweep
        bool collisionSweep = Physics.CapsuleCast(
            center + bounds, 
            center - bounds, 
            collider.radius, 
            sweepDir,
            out RaycastHit hit,
            dist, 
            collisionMask);

        if (collisionSweep)
        {
            Vector3 collisionNormal = hit.normal.normalized;
            // "Snap" the body to the collision surface
            float hitDistance = hit.distance;
            pathPosition += pathVelocity.normalized * hitDistance;
            // Offset from surface normal to prevent clipping in the next frame
            pathPosition += ProjectVecOntoPath(collisionNormal, t) * collisionResolutionOffset;
            
            // Resolved velocity "slides" up the colliding surfaceh
            worldVel = Vector3.ProjectOnPlane(worldVel, collisionNormal);
            pathVelocity = ProjectVecOntoPath(worldVel, t);
            
            // Gizmos debug
            collisionResolutionVel = worldVel;
            
            // Recursively resolve any remaining collisions
            ResolveCollisions();
        }
    }

    private void ApplyVelocity()
    {
        pathPosition += pathVelocity * Time.fixedDeltaTime;
        
        // Tangents are messed up at ends of a spline so clamp position to avoid that
        pathPosition.x = Mathf.Clamp(pathPosition.x, 0.01f, pathLength - 0.01f);
        float t = pathPosition.x / pathLength;
        UpdatePositionOnPath(t);
    }

    private void UpdatePositionOnPath(float t)
    {
        Vector3 worldPos = SplinePath.EvaluatePosition(t);
        transform.position = new Vector3(
            worldPos.x,
            pathPosition.y,
            worldPos.z
        );
    }
    
    /// <summary>
    /// Convert a direction vector from path space to world space
    /// </summary>
    /// <param name="splineVec"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    private Vector3 PathToWorldVec(Vector2 splineVec, float t)
    {
        Vector3 dir = SplinePath.EvaluateTangent(t);
        dir.y = 0;
        dir.Normalize();
        dir *= splineVec.x;
        return dir + Vector3.up * splineVec.y;
    }

    /// <summary>
    /// Project a direction vector onto the path
    /// </summary>
    /// <param name="worldVec"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    private Vector2 ProjectVecOntoPath(Vector3 worldVec, float t)
    {
        Vector3 horizontal = new Vector3(worldVec.x, 0, worldVec.z);
        Vector3 dir = SplinePath.EvaluateTangent(t);
        dir.y = 0;
        dir.Normalize();
        float x = Vector3.Dot(horizontal, dir);
        return new Vector2(x, worldVec.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 pos = transform.position;
        Gizmos.DrawLine(pos, pos + collisionResolutionVel);
    }
}
