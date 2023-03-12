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
    
    [ColorHeader("Collision", ColorHeaderColor.Config)]
    [SerializeField] private float collisionResolutionOffset;
    [SerializeField] private float collisionSweepBackOffset;
    [SerializeField] private int maxColliderResolves;
    [SerializeField] private LayerMask collisionMask;

    [ColorHeader("Grounding")]
    [SerializeField] private float groundProbeDistance;
    [SerializeField] private float groundSweepBackOffset;
    [SerializeField] private float maxGroundStableAngle;
    
    [ColorHeader("Physics State")]
    public Vector2 pathVelocity;
    public bool constrainVelocity;
    public GroundInfo currentGroundState;
    public GroundInfo prevGroundState;
    
    // Dependencies
    private PathTransform pathTransform;
    
    // Internal state
    private bool gravityEnabled;
    
    // Collider shape
    private float capsuleRadius;
    private Vector3 capsuleCenter;
    private Vector3 capsuleUpperPoint;
    private Vector3 capsuleLowerPoint;

    // Gizmo debugging fields
    private Vector3 collisionResolutionVel;
    private Vector3 splDir;

    private void OnValidate()
    {
        Validate();
    }

    private void Awake()
    {
        Validate();
    }
    
    private void Validate()
    {
        capsuleCollider.hideFlags = HideFlags.NotEditable;
        SetColliderDimensions(capsuleCollider.center, capsuleCollider.height, capsuleCollider.radius);
    }

    private void SetColliderDimensions(Vector3 center, float height, float radius)
    {
        capsuleCollider.center = center;
        capsuleCollider.height = height;
        capsuleCollider.radius = radius;

        capsuleRadius = radius;
        capsuleCenter = center;
        Vector3 hemisphereOffset = Vector3.up * (height / 2f - radius);
        capsuleUpperPoint = center + hemisphereOffset;
        capsuleLowerPoint = center - hemisphereOffset;
    }

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
        // Don't apply gravity if on stable ground
        if(gravityEnabled && !currentGroundState.isStableOnGround)
            pathVelocity.y -= Time.fixedDeltaTime * gravityAcceleration;
    }
    

    private void ResolveCollisions(ref float stepDist)
    {
        int currentResolves = 0;

        Vector3 currentMovePos = transform.position;
        
        while (currentResolves < maxColliderResolves)
        {
            Debug.Log("E");
            // Do some conversions to get velocity step for this fixedUpdate
            Vector3 worldVel = pathTransform.ProjectVectorFromPlane(pathVelocity);
            Vector3 sweepDir = worldVel.normalized;

            Vector3 closestHitNormal = default;
            Vector3 closestHitPoint = default;
            float closestHitDist = default;

            bool sweepHitObstacle = false;

            if (CapsuleOverlap(pathTransform.WorldPos, collisionMask, out Collider[] colliders) > 0)
            {
                float minDot = 0f;
                foreach (var overlapCollider in colliders)
                {
                    if(Physics.ComputePenetration(
                        capsuleCollider,
                        pathTransform.WorldPos,
                        Quaternion.identity,
                        overlapCollider,
                        overlapCollider.transform.position,
                        Quaternion.identity,
                        out Vector3 direction,
                        out float distance
                        ))
                    {
                        float dot = Vector3.Dot(direction, worldVel);
                        if (dot < minDot)
                        {
                            minDot = dot;
                            sweepHitObstacle = true;
                            closestHitDist = distance;
                            closestHitNormal = direction;
                            closestHitPoint = pathTransform.WorldPos + capsuleCenter - direction * distance;
                        }
                    }
                }
            }

            if (!sweepHitObstacle && CapsuleSweep(pathTransform.WorldPos, sweepDir, stepDist, collisionMask, out RaycastHit hit))
            {
                sweepHitObstacle = true;
                closestHitDist = hit.distance;
                closestHitNormal = hit.normal;
                closestHitPoint = hit.point;
            }

            if (sweepHitObstacle)
            {
                Vector3 collisionNormal = closestHitNormal;
            
                // Move the body to the collision surface
                float hitDistance = closestHitDist;
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

        if (GroundSweep(
                pathTransform.WorldPos,
                Vector3.down,
                groundProbeDistance,
                collisionMask,
                out RaycastHit groundHit))
        {
            if (IsHitStable(groundHit.normal))
            {
                currentGroundState.isStableOnGround = true;
            }

            currentGroundState.isTouchingGround = true;
            currentGroundState.groundNormalRaw = groundHit.normal;
            currentGroundState.groundNormal = pathTransform.ProjectVectorOntoPlane(groundHit.normal).normalized;
        }
    }

    private bool IsHitStable(Vector3 hitNormal)
    {
        float angle = Vector3.Angle(hitNormal, Vector3.up);
        return angle < maxGroundStableAngle;
    }

    private int CapsuleOverlap(Vector3 position, LayerMask hitMask, out Collider[] colliders)
    {
        var lower = position + capsuleLowerPoint;
        var upper = position + capsuleUpperPoint;

        var hits = Physics.OverlapCapsule(
            lower,
            upper,
            capsuleRadius,
            hitMask
        );

        colliders = hits;
        return hits.Length;
    }

    private bool CapsuleSweep(Vector3 position, Vector3 dir, float dist, LayerMask hitMask, out RaycastHit info)
    {
        var lower = position + capsuleLowerPoint - collisionSweepBackOffset * dir;
        var upper = position + capsuleUpperPoint - collisionSweepBackOffset * dir;

        bool hit = Physics.CapsuleCast(
            lower,
            upper,
            capsuleRadius,
            dir,
            out info,
            dist + collisionSweepBackOffset,
            hitMask
        );

        return hit;
    }

    private bool GroundSweep(Vector3 position, Vector3 dir, float dist, LayerMask hitMask, out RaycastHit info)
    {
        var lower = position + capsuleLowerPoint - groundSweepBackOffset * dir;
        var upper = position + capsuleUpperPoint - groundSweepBackOffset * dir;

        bool hit = Physics.CapsuleCast(
            lower,
            upper,
            capsuleRadius,
            dir,
            out info,
            dist + groundSweepBackOffset,
            hitMask
        );

        return hit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 pos = transform.position;
        Gizmos.DrawLine(pos, pos + collisionResolutionVel);
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(pos, pos + currentGroundState.groundNormalRaw);
    }
}
