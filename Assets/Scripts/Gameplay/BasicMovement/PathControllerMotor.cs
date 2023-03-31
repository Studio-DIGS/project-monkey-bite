using System;
using System.Collections.Generic;
using KinematicCharacterController;
using MushiCore.EditorAttributes;
using MushiCore.GizmoDrawer;
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

public class PathControllerMotor : MonoBehaviour, ICharacterController
{
    private const float epsilon = 0.0001f;

    [ColorHeader("Dependencies")]
    [SerializeField] private KinematicCharacterMotor motor;

    [ColorHeader("Config", ColorHeaderColor.Config)]
    [SerializeField] private float gravityAcceleration;

    [ColorHeader("Physics State")]
    public Vector2 pathVelocity;
    public CharacterGroundingReport CurrentGroundState => motor.GroundingStatus;

    // Dependencies
    private PathTransform pathTransform;
    
    // Internal state
    private bool gravityEnabled = true;

    public void Initialize(PathTransform pathTransform)
    {
        this.pathTransform = pathTransform;

        motor.CharacterController = this;
        
        // We don't want to auto sync the transform
        pathTransform.autoSyncTransform = false;
        gravityEnabled = true;
    }
    
    public void SetGravityEnabled(bool val) => gravityEnabled = val;
    public void SetForceUnground(bool forceUnground) => motor.SetForceUnground(forceUnground);

    public Vector2 ProjectNormalOntoSpline(Vector3 wNormal)
    {
        return pathTransform.ProjectVectorOntoPlane(wNormal).normalized;
    }

    public void TickPhysicsBody()
    {
        gizmoDrawer.Clear();
        ApplyForces();
        motor.UpdatePhase1(Time.fixedDeltaTime);
        motor.UpdatePhase2(Time.fixedDeltaTime);
        motor.Transform.SetPositionAndRotation(motor.TransientPosition, motor.TransientRotation);
    }
    
    private void ApplyForces()
    {
        if(gravityEnabled && !motor.LastGroundingStatus.IsStableOnGround)
            pathVelocity.y -= Time.fixedDeltaTime * gravityAcceleration;
    }
    
    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
         
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        Vector2 sTargetPos = pathTransform.Position + pathVelocity * deltaTime;

        Vector3 wStartPos = pathTransform.WorldPos;
        Vector3 wTargetPos = pathTransform.EvaluatePos(sTargetPos);

        Vector3 wStep = wTargetPos - wStartPos;

        motor.PlanarConstraintNormal = Vector3.Cross(Vector3.up, wStep).normalized;
    
        currentVelocity = (wStep) / deltaTime;
    }

    public void BeforeCharacterUpdate(float deltaTime)
    {
         motor.SetPosition(motor.transform.position);
    }

    public void PostGroundingUpdate(float deltaTime)
    {
         
    }

    public void AfterCharacterUpdate(float deltaTime)
    {         
        // Update spline position
        pathTransform.GetNearestPoint(motor.transform.position, out Vector3 wNearestPos, out Vector2 sNearestPos);
        pathTransform.Position = sNearestPos;
        
         // Update path velocity
         var currentVelocity = motor.BaseVelocity;

         float horizontalMag = Mathf.Sign(Vector2.Dot(currentVelocity, pathTransform.CTangent)) * new Vector2(currentVelocity.x, currentVelocity.z).magnitude;
         Vector2 sProjectedVel = new Vector2(horizontalMag, currentVelocity.y);
         
         pathVelocity = sProjectedVel;

         // Snap if too far
         if (Vector3.Distance(pathTransform.transform.position, wNearestPos) > 0.001f)
         {
             motor.SetPosition(wNearestPos);
         }
         
         gizmoDrawer.Add(new GizmoDrawerWireSphere(
             Color.cyan,
             pathTransform.WorldPos,
             0.04f
             ));

         var position = motor.Transform.position;
         
         gizmoDrawer.Add(new GizmoDrawerWireSphere(
             Color.green,
             position,
             0.06f
             ));
         
         gizmoDrawer.Add(new GizmoDrawerLine(
             Color.magenta,
             position,
             position + motor.GroundingStatus.GroundNormal
             ));
         gizmoDrawer.Add(new GizmoDrawerLine(
             Color.yellow,
             position,
             position + motor.GroundingStatus.InnerGroundNormal
         ));
         gizmoDrawer.Add(new GizmoDrawerLine(
             Color.red,
             position,
             position + motor.GroundingStatus.OuterGroundNormal
         ));
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        return true;
    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
        
    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
         
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {
         
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {
         
    }
    
    private GizmoDrawer gizmoDrawer = new();

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        gizmoDrawer.Draw();
    }
#endif
}
