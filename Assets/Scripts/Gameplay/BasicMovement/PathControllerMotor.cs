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

    [ColorHeader("Config")]
    [SerializeField] private PathControllerMotorProfileSO PCCProfile;

    [ColorHeader("Physics State Debug")]
    public Vector2 pathVelocity;
    public GroundedState CurrentGroundState;
    public GroundedState PreviousGroundState;
    public bool ForceUnground => forceUnground;
    
    // Internal
    private bool gravityEnabled;
    private bool forceUnground = false;
    private PathTransform pathTransform;
    
    // Debug
    private int stepNumber;

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

    public void TickPhysicsBody(float deltaTime)
    {
        gizmoDrawer.Clear();
        stepNumber = 0;
        ApplyForces();
        
        int moveIterations = Mathf.Max(PCCProfile.MinMoveIterations, Mathf.CeilToInt(Mathf.Abs(pathVelocity.x) * deltaTime / PCCProfile.MaxMoveIterationLength));
        float moveIterationTimeStep = deltaTime / moveIterations;

        for (int i = 0; i < moveIterations; i++)
        {
            if (pathVelocity.sqrMagnitude == 0)
            {
                break;
            }
            Vector2 p1 = pathTransform.Position;
            PerformMove(moveIterationTimeStep);
            gizmoDrawer.Add(pathTransform.GetGizmoLine(gizmoDrawer.GetCycledColor(), p1, pathTransform.Position));
        }

        PreviousGroundState = CurrentGroundState;
        PerformGroundProbing();
        
        pathTransform.SyncWorldPosition();
    }

    private void ApplyForces()
    {
        if(gravityEnabled && !CurrentGroundState.IsStableOnGround)
            pathVelocity.y -= Time.fixedDeltaTime * PCCProfile.GravityAcceleration;
    }

    private void PerformMove(float timeStep)
    {
        int stepCollisionResolveCount = 0;
        
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

        while (stepCollisionResolveCount < PCCProfile.MaxColliderResolves && sTransientStepDist > epsilon)
        {
            stepNumber++;
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

            // Overlap Check
            
            if(CapsuleOverlap(wStepStartPos,  PCCProfile.CollisionMask, ref colliderBuffer, out int hitCount))
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
                        Vector2 sNorm = ProjectNormalAgainstStepPlane(sTransientPos, wTempStepDir, resolutionDirection, Mathf.Sign(sTransientStepDir.x));

                        bool crease = transientSweepState == ObstacleSweepState.SuccessiveHit && EvaluateCrease(sTransientStepDir, sNorm, sPrevObstacleNormal);
                        
                        if (crease || dot < 0 && dot < mostObstructingDot)
                        {
                            mostObstructingDot = dot;
                            obstacleFound = true;
                            wObstacleNormal = resolutionDirection;
                            sToObstacleSurfaceDir = sNorm;
                            obstacleDist = resolutionDistance;

                            isEjection = true;
                        }
                    }
                }
            }
            
            // Cast

            if (!obstacleFound && CapsuleSweep(
                    wStepStartPos,
                    wTempStepDir, 
                    wTempStepDist, 
                    out RaycastHit hit,
                    PCCProfile.CollisionMask))
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
                Vector2 sObstacleNormal = ProjectNormalAgainstStepPlane(
                    sTransientPos, 
                    wTempStepDir, 
                    wObstacleNormal, 
                    Mathf.Sign(sTransientStepDir.x));
                
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
                sTransientPos += projectionSurfaceNormal * PCCProfile.CollisionResolutionOffset;
                
                // Save prev obstacle state
                sPrevObstacleNormal = sObstacleNormal;
                prevStableOnObstacle = isStableOnObstacle;
            }
            else
            {
                break;
            }

            stepCollisionResolveCount++;
        }

        if (stepCollisionResolveCount >= PCCProfile.MaxColliderResolves)
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

    private void PerformGroundProbing()
    {
        var transientGroundState = new GroundedState()
        {
            FoundAnyGround = false,
            IsStableOnGround = false,
            GroundNormal = Vector2.up,
            GroundPoint = Vector2.zero
        };

        if (ForceUnground)
        {
            CurrentGroundState = transientGroundState;
            return;
        }
        
        // Probe sweep state
        Vector2 sTransientProbePos = pathTransform.Position;
        Vector2 sTransientProbeDir = Vector2.down;
        float sTransientProbeDist = PCCProfile.GroundProbeDistance;
        int groundProbeIterations = 0;
        
        bool launchOffLedge = pathVelocity.magnitude > PCCProfile.LaunchOffLedgeVelocity;

        if (!CurrentGroundState.IsStableOnGround)
        {
            sTransientProbeDist = epsilon + PCCProfile.GroundTouchDistance;
        }

        if (launchOffLedge)
        {
            sTransientProbeDist = epsilon;
        }

        while (sTransientProbeDist > 0 && groundProbeIterations < PCCProfile.MaxGroundProbeIterations)
        {
            // Calculate straight-line step 
            sTransientProbePos.x = pathTransform.LoopX(sTransientProbePos.x);
            
            Vector2 sStepTargetPos = sTransientProbePos + sTransientProbeDir * sTransientProbeDist;

            Vector3 wStepStartPos = pathTransform.EvaluatePos(sTransientProbePos);
            Vector3 wStepTargetPos = pathTransform.EvaluatePos(sStepTargetPos);
            
            Vector3 wTempStep = wStepTargetPos - wStepStartPos;
            Vector3 wTempStepDir = wTempStep.normalized;
            float wTempStepDist = wTempStep.magnitude;
            
            bool groundSweep = CapsuleSweep(
                wStepStartPos,
                wTempStepDir,
                sTransientProbeDist,
                out RaycastHit groundProbeHit,
                PCCProfile.CollisionMask,
                -epsilon
            );

            if (groundSweep)
            {
                // Move the probe position to the surface
                Vector2 snapVec = groundProbeHit.distance * sTransientProbeDir;
                sTransientProbePos += snapVec;

                // Reduce distance accordingly
                sTransientProbeDist -= groundProbeHit.distance;

                // Use raycast to get raw normal
                // This is because capsulesweep results in a smooth normal on sharp edges, which causes issues
                // When walking off ledges due to ground snapping
                var rawNormalProbe = Physics.Raycast(
                    wStepStartPos,
                    wTempStepDir,
                    out RaycastHit rawNormalHit,
                    sTransientProbeDist,
                    PCCProfile.CollisionMask
                );

                Vector2 sGroundProbeCapsuleNormal = ProjectNormalAgainstStepPlane(
                    sTransientProbePos, 
                    wTempStepDir, 
                    groundProbeHit.normal, 
                    Mathf.Sign(sTransientProbeDir.x));
                
                Vector2 sGroundProbeRayNormal = ProjectNormalAgainstStepPlane(
                    sTransientProbePos, 
                    wTempStepDir, 
                    rawNormalHit.normal, 
                    Mathf.Sign(sTransientProbeDir.x));
                
                          
                gizmoDrawer.Add(pathTransform.GetGizmoLine(
                    Color.red, sTransientProbePos, sTransientProbePos + sGroundProbeCapsuleNormal));
                
                gizmoDrawer.Add(pathTransform.GetGizmoLine(
                    Color.blue, sTransientProbePos, sTransientProbePos + sGroundProbeRayNormal));
                
                // Are we moving towards the normal? (ledge direction)
                // Due to capsulesweep giving smooth normals this will be used to detect if walking off ledge or not
                bool steppingOffLedge =
                    CurrentGroundState.IsStableOnGround &&
                    (sGroundProbeCapsuleNormal.x * pathVelocity.x > 0);

                // Evaluate the ground hit
                if (!steppingOffLedge)
                {
                    EvaluateGroundProbeHit(wTempStepDir, groundProbeHit.point,
                        sTransientProbePos, sGroundProbeCapsuleNormal, ref transientGroundState);
                }
                else
                {
                    EvaluateGroundProbeHit(wTempStepDir, groundProbeHit.point,
                        sTransientProbePos, sGroundProbeRayNormal, ref transientGroundState);
                }

                if (!launchOffLedge && transientGroundState.IsStableOnGround)
                {
                    // Snap to ground
                    float offset = PCCProfile.CollisionResolutionOffset;
                    if (CurrentGroundState.IsStableOnGround && groundProbeHit.distance > offset)
                    {
                        pathTransform.Position = sTransientProbePos - sTransientProbeDir * offset;
                    }
                    break;
                }

                // Collide and slide the probe step
                Vector2 projectedProbeDir = sTransientProbeDir.ProjectOntoPlane(transientGroundState.GroundNormal);
                sTransientProbeDist *= projectedProbeDir.magnitude;
                sTransientProbeDir = projectedProbeDir.normalized;
            }
            else
            {
                break;
            }
            
            groundProbeIterations++;
        }

        if (!CurrentGroundState.IsStableOnGround && transientGroundState.IsStableOnGround)
        {
            // Landing
            pathVelocity = pathVelocity.ProjectOntoPlane(Vector2.up);
            pathVelocity = pathVelocity.RedirectOntoPlane(transientGroundState.GroundNormal);
        }
        
        CurrentGroundState = transientGroundState;
    }

    private void EvaluateGroundProbeHit(
        Vector3 wHitPosition,
        Vector2 wHitDirection,
        Vector2 sTransientProbePos, 
        Vector2 sGroundProbeHitNormal, 
        ref GroundedState transientGroundState)
    {
        bool isStableOnGroundHit = IsStableOnObstacle(sGroundProbeHitNormal);
        
        transientGroundState.FoundAnyGround = true;
        transientGroundState.GroundNormal = sGroundProbeHitNormal;
        transientGroundState.IsStableOnGround = isStableOnGroundHit;
    }

    private Vector2 ProjectNormalAgainstStepPlane(Vector3 sTransientProbePos, Vector3 wStepDir, Vector3 wNormal, float sXDir)
    {
        Vector2 wPlaneTangentHorizontal = new Vector2(wStepDir.x, wStepDir.z);
        Vector3 wProjectPlaneTangent = wStepDir;
                
        if (wPlaneTangentHorizontal.sqrMagnitude < epsilon)
            wProjectPlaneTangent = pathTransform.EvaluateTangent(sTransientProbePos);
                
        Vector2 sProjectedNormal = ProjectVectorOntoPlaneSpace(wNormal, wProjectPlaneTangent).normalized;
                
        sProjectedNormal.x *= sXDir;
        return sProjectedNormal;
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
                    sPrevObstacleNormal
                ))
            {
                // A crease in 2D space means immediate stopping all movement
                // Also update the obstacle normal to be opposite to the movement step
                sweepState = ObstacleSweepState.AfterCreaseHit;
                sProjectionSurfaceNormal = (sPrevObstacleNormal + sObstacleNormal).normalized;
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
                projectedStepDir = projectedStepDir.ProjectOntoPlane(Vector2.up);
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
        if (sObstacleNormal != Vector2.zero && Vector2.Angle(sObstacleNormal, Vector2.up) < PCCProfile.MaxStableAngle)
        {
            return true;
        }
        return false;
    }

    private bool EvaluateCrease(
        Vector2 sCurrentStepDir,
        Vector2 sObstacleNormal,
        Vector2 sPrevObstacleNormal
    )
    {
        float surfacesDot = Vector2.Dot(sObstacleNormal, sPrevObstacleNormal);
        // Check if the surfaces form a crease
        if (surfacesDot <= 0)
        {
            // If the last two obstacles form a crease, we can assume the player was moving into it 
            // May need an extra check if creases are detected in wrong places
            return true;
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

    public bool CapsuleSweep(Vector3 wPos, Vector3 sweepDir, float stepDist, out RaycastHit info, LayerMask layerMask, float radiusAdjust = 0f)
    {
        var collider = capsuleCollider;
        // Get bounds of the collider
        float radius = collider.radius + radiusAdjust;
        Vector3 center = wPos + collider.center;
        Vector3 pointOffset = collider.transform.up * (collider.height/2f - radius);

        float sweepOffset = PCCProfile.SweepOffset;
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
            layerMask);

        info.distance -= sweepOffset;
        return collisionSweep;
    }

    public bool CapsuleOverlap(Vector3 wPos, LayerMask layerMask, ref Collider[] colliders, out int hitCount, float radiusAdjust = 0)
    {
        var collider = capsuleCollider;
        // Get bounds of the collider
        float radius = collider.radius + radiusAdjust;
        Vector3 center = wPos + collider.center;
        Vector3 pointOffset = collider.transform.up * (collider.height/2f - radius);

        var overlapCount = Physics.OverlapCapsuleNonAlloc(
            center + pointOffset,
            center - pointOffset,
            radius,
            colliders,
            layerMask
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
