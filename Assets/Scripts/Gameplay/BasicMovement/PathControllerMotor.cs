using System;
using MushiCore.EditorAttributes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[System.Serializable]
public struct GroundInfo
{
    public bool isStableOnGround;
    public bool isTouchingGround;
    public Vector2 groundNormal;
    public Vector3 groundNormalRaw;
}

public class PathControllerMotor : MonoBehaviour
{
    [ColorHeader("Dependencies")]
    [SerializeField] private CapsuleCollider capsuleCollider;

    [ColorHeader("Config", ColorHeaderColor.Config)]
    [SerializeField] private float gravityAcceleration;
    [SerializeField] private float collisionResolutionOffset;
    [SerializeField] private float sweepOffset;
    [SerializeField] private int maxColliderResolves;
    [SerializeField] private LayerMask collisionMask;

    // Fields
    // TODO: Cache t instead of calculating all over the place
    [ColorHeader("Physics State")]
    public Vector2 pathVelocity;
    public bool constrainVelocity;
    public GroundInfo currentGroundState;
    public GroundInfo prevGroundState;
    
    private bool gravityEnabled;
    
    // Properties
    private PathTransform pathTransform;

    // Gizmo debugging fields
    private Vector3 collisionResolutionVel;
    private Vector3 splDir;

    public void Initialize(PathTransform pathTransform)
    {
        this.pathTransform = pathTransform;
        // We don't want to auto sync the transform
        pathTransform.autoSyncTransform = false;
        pathTransform.SnapToNearestPoint(transform.position);
        pathTransform.SyncTransform();
        gravityEnabled = true;
    }
    
    public void SetGravityEnabled(bool val) => gravityEnabled = val;

    public void TickPhysicsBody()
    {
        ApplyForces();
        if (constrainVelocity)
        {
            pathVelocity = Vector2.zero;
        }
        float stepDist = pathVelocity.magnitude * Time.fixedDeltaTime;
        ResolveCollisions(ref stepDist);
        ApplyVelocity(stepDist);
        UpdateGrounding();
    }
    
    private void ApplyForces()
    {
        if(gravityEnabled)
            pathVelocity.y -= Time.fixedDeltaTime * gravityAcceleration;
    }
    

    private void ResolveCollisions(ref float stepDist)
    {
        int currentResolves = 0;
        
        while (currentResolves < maxColliderResolves)
        {
            // Do some conversions to get velocity step for this fixedUpdate
            Vector3 worldVel = pathTransform.ProjectVectorFromPlane(pathVelocity);
            Vector3 sweepDir = worldVel.normalized;

            bool collisionSweep = CapsuleSweep(capsuleCollider, sweepDir, stepDist, out RaycastHit hit);

            if (collisionSweep)
            {
                Vector3 collisionNormal = hit.normal.normalized;
            
                // Move the body to the collision surface
                float hitDistance = hit.distance - sweepOffset;
                Vector2 snapVec = pathVelocity.normalized * hitDistance;

                pathTransform.Position += snapVec;
            
                // Snapping is simulating velocity for this step, so reduce the step dist to match
                stepDist -= hitDistance;

                // Offset from surface normal to prevent clipping in the next frame
                pathTransform.Position += pathTransform.ProjectVectorOntoPlane(collisionNormal).normalized * collisionResolutionOffset;
            
                // Resolved velocity "slides" up the colliding surface
                worldVel = Vector3.ProjectOnPlane(worldVel, collisionNormal);
                float pathVelMag = pathVelocity.magnitude;
                pathVelocity = pathTransform.ProjectVectorOntoPlane(worldVel);
            
                // This hit and slide reduces velocity, so reduce step distance accordingly
                stepDist *= pathVelocity.magnitude / pathVelMag;
            
                // Gizmos debug
                collisionResolutionVel = worldVel;
            }
            else
            {
                break;
            }

            currentResolves++;
        }

        if (currentResolves >= maxColliderResolves)
        {
            Debug.Log("TOO MANY RESOLVES");
            pathVelocity = Vector2.zero;
        }
    }
    
    private void ApplyVelocity(float stepDist)
    {
        Vector2 velStep = pathVelocity.normalized * stepDist;
        pathTransform.Position += velStep;
        pathTransform.SyncTransform();
    }
    
    private void UpdateGrounding()
    {
        prevGroundState = currentGroundState;
        currentGroundState = new GroundInfo()
        {
            isTouchingGround = false,
            isStableOnGround = false,
            groundNormal = Vector2.up,
            groundNormalRaw = Vector3.up
        };
    }


    private bool CapsuleSweep(CapsuleCollider collider, Vector3 sweepDir, float stepDist, out RaycastHit info)
    {
        // Get bounds of the collider
        Vector3 center = collider.bounds.center;
        Vector3 bounds = collider.transform.up * (collider.height/2f - collider.radius);

        Vector3 sweepEnd = center + sweepDir * stepDist;
        Debug.DrawLine(center + bounds, center - bounds, Color.magenta);
        
        // Offset the sweep backwards to avoid starting the cast inside any obstacles
        center -= sweepDir * sweepOffset;
        
        // Perform sweep
        bool collisionSweep = Physics.CapsuleCast(
            center + bounds, 
            center - bounds, 
            collider.radius, 
            sweepDir,
            out info,
            stepDist + sweepOffset, 
            collisionMask);

        return collisionSweep;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 pos = transform.position;
        Gizmos.DrawLine(pos, pos + collisionResolutionVel);
    }
}
