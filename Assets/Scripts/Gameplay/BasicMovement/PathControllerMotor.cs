using System;
using System.Collections.Generic;
using MushiCore.EditorAttributes;
using MushiCore.GizmoDrawer;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[System.Serializable]
public struct GroundedState
{
    public bool FoundAnyGround;
    public bool IsStableOnGround;
    public Vector2 GroundNormal;
    public Vector2 GroundPoint;
}

public class PathControllerMotor : MonoBehaviour
{
    private const float epsilon = 0.00001f;

    [ColorHeader("Dependencies")]
    [SerializeField] private CapsuleCollider capsuleCollider;

    [ColorHeader("Config", ColorHeaderColor.Config)]
    [SerializeField] private float gravityAcceleration;
    [SerializeField] private LayerMask collisionMask;
    
    [ColorHeader("Collision Resolution Config", ColorHeaderColor.Config)]
    [SerializeField] private float collisionResolutionOffset;
    [SerializeField] private float sweepOffset;
    [SerializeField] private int maxColliderResolves;

    [ColorHeader("Grounding Config", ColorHeaderColor.Config)]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float maxStableAngle;

    [ColorHeader("Physics State Debug")]
    public Vector2 pathVelocity;
    public GroundedState CurrentGroundState;
    public GroundedState PreviousGroundState;
    public bool ForceUnground => forceUnground;
    
    // Internal
    private bool gravityEnabled;
    private bool forceUnground = false;
    private PathTransform pathTransform;

    private enum ObstacleSweepState
    {
        InitialHit,
        SuccessiveHit,
        AfterCreaseHit
    }

    public void Initialize(PathTransform pathTransform)
    {
        this.pathTransform = pathTransform;

        CurrentGroundState.IsStableOnGround = true;
        CurrentGroundState.GroundNormal = Vector3.up;

        // We don't want to auto sync the transform
        pathTransform.autoSyncTransform = false;
        gravityEnabled = true;
    }
    
    public void SetGravityEnabled(bool val) => gravityEnabled = val;
    public void SetForceUnground(bool val) => forceUnground = val;

    public Vector2 ProjectNormalOntoSpline(Vector3 wNormal)
    {
        return pathTransform.ProjectToPathSpace(wNormal).normalized;
    }

    public void TickPhysicsBody()
    {
        gizmoDrawer.Clear();
        ApplyForces();
        PerformMove(Time.fixedDeltaTime);
        
        PreviousGroundState = CurrentGroundState;
        PerformGroundingCheck();
        
        pathTransform.SyncWorldPosition();
    }

    private void ApplyForces()
    {
        if(gravityEnabled && !CurrentGroundState.IsStableOnGround)
            pathVelocity.y -= Time.fixedDeltaTime * gravityAcceleration;
    }

    private void PerformMove(float timeStep)
    {
        int resolveCount = 0;
        
        // Transient state
        Vector2 sTransientPos = pathTransform.Position;
        Vector2 sTransientVel = pathVelocity;
        Vector2 sTransientStepDir = pathVelocity.normalized;
        float sTransientStepDist = pathVelocity.magnitude * timeStep;

        ObstacleSweepState transientSweepState = ObstacleSweepState.InitialHit;
        
        // Obstacle state cache
        Vector2 sPrevObstacleNormal = default;
        Vector2 sPrevStepDir = default;
        bool prevStableOnObstacle = false;

        var colliderBuffer = new Collider[16];

        while (resolveCount < maxColliderResolves && sTransientStepDist > epsilon)
        {
            // Sweep obstacle data
            Vector3 wObstacleNormal = default;
            Vector2 sToObstacleSurfaceDir = default;
            float obstacleDist = 0f;
            bool obstacleFound = false;
            bool isEjection = false;
            
            // Calculate straight-line step 
            sTransientPos.x = pathTransform.LoopX(sTransientPos.x);
            
            Vector2 sStepTargetPos = sTransientPos + sTransientStepDir * sTransientStepDist;

            Vector3 wStepStartPos = pathTransform.EvaluatePos(sTransientPos);
            Vector3 wStepTargetPos = pathTransform.EvaluatePos(sStepTargetPos);
            
            Vector3 wTempStep = wStepTargetPos - wStepStartPos;
            Vector3 wTempStepDir = wTempStep.normalized;
            float wTempStepDist = wTempStep.magnitude;

            // Is the step going in positive or negative x spline direction?
            int sStepFacing = (int)Mathf.Sign(sTransientStepDir.x);
            
            // Overlap Check
            
            if(CapsuleOverlap(capsuleCollider, wStepStartPos, ref colliderBuffer, out int hitCount))
            {
                float mostObstructingDot = float.MaxValue;
                
                for (int i = 0;i < hitCount;i++)
                {
                    var overlapCollider = colliderBuffer[i];
                    if (Physics.ComputePenetration(
                            capsuleCollider,
                            wStepStartPos,
                            Quaternion.identity,
                            overlapCollider,
                            overlapCollider.transform.position,
                            overlapCollider.transform.rotation,
                            out Vector3 resolutionDirection,
                            out float resolutionDistance
                        ))
                    {
                        float dot = Vector3.Dot(resolutionDirection, wTempStep);

                        if (dot < 0f && dot < mostObstructingDot)
                        {
                            mostObstructingDot = dot;
                            obstacleFound = true;
                            wObstacleNormal = resolutionDirection;
                            sToObstacleSurfaceDir = ProjectVectorOntoPlaneSpace(wObstacleNormal, sTransientStepDir);
                            sToObstacleSurfaceDir.x *= sStepFacing;
                            obstacleDist = resolutionDistance;

                            isEjection = true;
                        }
                    }
                }
            }
            
            // Cast

            if (!obstacleFound && CapsuleSweep(
                    capsuleCollider, 
                    wStepStartPos,
                    wTempStepDir, 
                    wTempStepDist, 
                    out RaycastHit hit))
            {
                obstacleFound = true;
                wObstacleNormal = hit.normal;
                sToObstacleSurfaceDir = sTransientStepDir;
                obstacleDist = hit.distance;
                isEjection = false;
            }

            if (obstacleFound)
            {
                // Move the body to the collision surface
                Vector2 snapVec = sToObstacleSurfaceDir * obstacleDist;
                sTransientPos += snapVec;
            
                // If the player was not already overlapping, then this counts as part of the step
                if (!isEjection)
                    sTransientStepDist -= obstacleDist;

                // Project obstacle normal into spline space
                Vector2 wPlaneTangentHorizontal = new Vector2(wTempStepDir.x, wTempStepDir.z);
                Vector3 wProjectPlaneTangent = wTempStepDir;
                if (wPlaneTangentHorizontal.sqrMagnitude < epsilon)
                    wProjectPlaneTangent = pathTransform.EvaluateTangent(sTransientPos);
                
                Vector2 sObstacleNormal = ProjectVectorOntoPlaneSpace(wObstacleNormal, wProjectPlaneTangent).normalized;

                sObstacleNormal.x *= sStepFacing;

                gizmoDrawer.Add(pathTransform.GetGizmoLine(Color.green, sTransientPos, sTransientPos + sObstacleNormal));

                bool isStableOnObstacle = IsStableOnObstacle(sObstacleNormal);

                // Update previous state
                sPrevStepDir = sTransientStepDir;
                
                // Velocity & Step projection
                
                ProjectMoveStepAgainstObstacle(
                    ref transientSweepState,
                    isStableOnObstacle,
                    sObstacleNormal,
                    sTransientStepDir,
                    prevStableOnObstacle,
                    sPrevObstacleNormal,
                    sPrevStepDir,
                    ref sTransientStepDir,
                    ref sTransientStepDist,
                    ref  sTransientVel,
                    out Vector2 projectionSurfaceNormal
                    );
                
                // Offset from projection surface
                sTransientPos += projectionSurfaceNormal * collisionResolutionOffset;
                
                // Save prev obstacle state
                sPrevObstacleNormal = sObstacleNormal;
                prevStableOnObstacle = isStableOnObstacle;
            }
            else
            {
                break;
            }

            resolveCount++;
        }

        if (resolveCount >= maxColliderResolves)
        {
            sTransientVel = Vector2.zero;
        }

        if (sTransientStepDist <= epsilon)
        {
            sTransientStepDist = 0f;
        }
        
        sTransientPos += sTransientStepDir * sTransientStepDist;
        
        // Update to transient state
        pathVelocity = sTransientVel;
        pathTransform.Position = sTransientPos;
    }

    private void PerformGroundingCheck()
    {
        var groundState = new GroundedState()
        {
            FoundAnyGround = false,
            IsStableOnGround = false,
            GroundNormal = Vector2.up,
            GroundPoint = Vector2.zero
        };

        bool groundSweep = CapsuleSweep(
            capsuleCollider,
            pathTransform.WorldPos,
            Vector3.down,
            groundCheckDistance + collisionResolutionOffset + epsilon,
            out RaycastHit info,
            -epsilon
        );

        if (groundSweep && !ForceUnground)
        {
            groundState.FoundAnyGround = true;
            Vector3 wNormal = info.normal;
            Vector2 sNormal = pathTransform.ProjectToPathSpace(wNormal);

            groundState.GroundNormal = sNormal;
            groundState.IsStableOnGround = IsStableOnObstacle(sNormal);
        }

        CurrentGroundState = groundState;
    }

    private void ProjectMoveStepAgainstObstacle(
        ref ObstacleSweepState sweepState,
        bool isStableOnHit, 
        Vector2 sObstacleNormal, 
        Vector2 sStepDir,
        bool wasPrevStableOnHit,
        Vector2 sPrevObstacleNormal,
        Vector3 sPrevStepDir,
        ref Vector2 sTransientStepDir,
        ref float sTransientStepDist,
        ref Vector2 sTransientVel,
        out Vector2 sProjectionSurfaceNormal
    )
    {
        if (sweepState == ObstacleSweepState.InitialHit)
        {
            HandleMoveStepProjection(ref sTransientStepDir, ref sTransientStepDist, ref sTransientVel, sObstacleNormal, isStableOnHit);
            sProjectionSurfaceNormal = sObstacleNormal;
            sweepState = ObstacleSweepState.SuccessiveHit;
        }
        else if (sweepState == ObstacleSweepState.SuccessiveHit)
        {
            if (EvaluateCrease(
                sTransientStepDir,
                sObstacleNormal,
                sPrevObstacleNormal,
                isStableOnHit,
                wasPrevStableOnHit
                ))
            {
                // A crease in 2D space means immediate stopping all movement
                // Also update the obstacle normal to be opposite to the movement step
                sweepState = ObstacleSweepState.AfterCreaseHit;
                sProjectionSurfaceNormal = -sTransientStepDir;
                sTransientStepDist = 0f;
                sTransientVel = Vector2.zero;
            }
            else
            {
                HandleMoveStepProjection(ref sTransientStepDir, ref sTransientStepDist, ref sTransientVel, sObstacleNormal, isStableOnHit);
                sProjectionSurfaceNormal = sObstacleNormal;
            }
        }
        else
        {
            // This shouldn't ever be reached but uhhhh
            // If it does then stop all movement
            sProjectionSurfaceNormal = -sTransientStepDir;
            sTransientStepDist = 0f;
            sTransientVel = Vector2.zero;
        }
    }

    private void HandleMoveStepProjection(ref Vector2 sStepDir, ref float sStepDist, ref Vector2 sTransientVel, Vector2 sObstacleNormal, bool isStableOnHit)
    {
        Vector2 projectedStepDir = sStepDir;
        if (CurrentGroundState.IsStableOnGround && !ForceUnground)
        {
            // Stable obstacle is lossless redirect (moving up stable slopes)
            if (isStableOnHit)
            {
                projectedStepDir = projectedStepDir.RedirectOntoPlane(sObstacleNormal);
            }
            // Otherwise keep the direction stuck to the ground (prevents "humping" steep slopes while stable on the ground)
            else
            {
                projectedStepDir = projectedStepDir.ProjectOntoPlane(sObstacleNormal);
                projectedStepDir = projectedStepDir.ProjectOntoPlane(CurrentGroundState.GroundNormal);
            }
        }
        else
        {
            // Landing onto stable ground, keep the pure horizontal and lossless redirect on surface
            if (isStableOnHit)
            {
                projectedStepDir = projectedStepDir.ProjectOntoPlane(Vector3.up);
                projectedStepDir = projectedStepDir.RedirectOntoPlane(sObstacleNormal);
            }
            // Hitting an obstacle while unstable (simple projection)
            else
            {
                projectedStepDir = sStepDir.ProjectOntoPlane(sObstacleNormal);
            }
        }

        // Reassign values back to state
        float projectMagRatio = projectedStepDir.magnitude;
        
        sStepDist *= projectMagRatio;
        sStepDir = projectedStepDir.normalized;
        sTransientVel = sStepDir * (sTransientVel.magnitude * projectMagRatio);
    }

    private bool IsStableOnObstacle(Vector2 sObstacleNormal)
    {
        if (Vector2.Angle(sObstacleNormal, Vector2.up) < maxStableAngle)
        {
            return true;
        }
        return false;
    }

    private bool EvaluateCrease(
        Vector2 sCurrentStepDir,
        Vector2 sObstacleNormal,
        Vector2 sPrevObstacleNormal,
        bool isHitStable,
        bool isPrevHitStable
    )
    {
        float surfacesDot = Vector2.Dot(sObstacleNormal, sPrevObstacleNormal);

        /*Debug.Log($"{sObstacleNormal} current");
        Debug.Log($"{sPrevObstacleNormal} prev");
        Debug.Break();*/
        // Check if the surfaces form a crease
        if (surfacesDot <= 0)
        {
            // Check if the current step is moving into the crease
            float stepDot = Vector2.Dot(sObstacleNormal, sCurrentStepDir);
            if (stepDot <= 0)
            {
               
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Project a vector onto a 2d space spanned by Vector3.up and a plane tangent's horizontal
    /// </summary>
    /// <param name="wToProject"></param>
    /// <param name="wPlaneTangent"></param>
    /// <returns></returns>
    private Vector2 ProjectVectorOntoPlaneSpace(Vector3 wToProject, Vector3 wPlaneTangent)
    {
        float cachedNormalY = wToProject.y;
        Vector2 toProjectHorizontal = new Vector2(wToProject.x, wToProject.z);
        Vector2 wTangentHorizontal = new Vector2(wPlaneTangent.x, wPlaneTangent.z).normalized;
        Vector2 sProjectedHorizontal = Vector2.Dot(toProjectHorizontal, wTangentHorizontal) * Vector2.right;
        sProjectedHorizontal.y = cachedNormalY;

        return sProjectedHorizontal;
    }

    private bool CapsuleSweep(CapsuleCollider collider, Vector3 wPos, Vector3 sweepDir, float stepDist, out RaycastHit info, float radiusAdjust = 0f)
    {
        // Get bounds of the collider
        float radius = collider.radius + radiusAdjust;
        Vector3 center = wPos + collider.center;
        Vector3 pointOffset = collider.transform.up * (collider.height/2f - radius);

        // Offset the sweep backwards to avoid starting the cast inside any obstacles
        center -= sweepDir * sweepOffset;
        
        // Perform sweep
        bool collisionSweep = Physics.CapsuleCast(
            center + pointOffset, 
            center - pointOffset, 
            radius, 
            sweepDir,
            out info,
            stepDist + sweepOffset, 
            collisionMask);

        info.distance -= sweepOffset;
        return collisionSweep;
    }

    private bool CapsuleOverlap(CapsuleCollider collider, Vector3 wPos, ref Collider[] colliders, out int hitCount, float radiusAdjust = 0)
    {
        // Get bounds of the collider
        float radius = collider.radius + radiusAdjust;
        Vector3 center = wPos + collider.center;
        Vector3 pointOffset = collider.transform.up * (collider.height/2f - radius);

        var overlapCount = Physics.OverlapCapsuleNonAlloc(
            center + pointOffset,
            center - pointOffset,
            radius,
            colliders,
            collisionMask
        );
        hitCount = overlapCount;
        return overlapCount > 0;
    }

    private GizmoDrawer gizmoDrawer = new();

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        gizmoDrawer.Draw();
    }
#endif
}
