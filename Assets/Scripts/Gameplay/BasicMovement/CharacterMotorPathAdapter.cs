using System;
using System.Collections.Generic;
using KinematicCharacterController;
using MushiCore.EditorAttributes;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class CharacterMotorPathAdapter : MonoBehaviour, ICharacterController
{
    [ColorHeader("Config")]
    public float gravityAccel;
    
    [ColorHeader("Debug")]
    public Vector2 pathVelocity;

    private KinematicCharacterMotor motor;
    private PathTransform pathTransform;

    private bool gravityEnabled = true;

    private Vector3 stepDir;
    private Vector3 stepVel;

    public Vector2 projectedNormal;

    public CharacterGroundingReport groundState => motor.GroundingStatus;

    public void SetGravityEnabled(bool val)
    {
        gravityEnabled = val;
    }
    
    public void Initialize(KinematicCharacterMotor motor, PathTransform pathTransform)
    {
        this.motor = motor;
        this.pathTransform = pathTransform;
        motor.CharacterController = this;
    }
    
    public void BeforeCharacterUpdate(float deltaTime)
    {
        motor.SetPosition(pathTransform.WorldPos);

        if (gravityEnabled)
            pathVelocity.y -= deltaTime * gravityAccel;

        Vector3 initialPos = transform.position;
        Vector2 targetPathPos = pathTransform.Position + pathVelocity * deltaTime;

        Vector3 targetPos = pathTransform.EvaluatePos(targetPathPos);

        Vector3 step = targetPos - initialPos;
        stepDir = step.normalized;
        stepVel = step / deltaTime;
    }
    
    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        currentVelocity = stepVel;
    }
    
    public void AfterCharacterUpdate(float deltaTime)
    {
        pathTransform.SnapToNearestPoint(motor.TransientPosition, false);

        Vector3 velocity = motor.Velocity;
        float cachedY = velocity.y;
        velocity.y = 0f;

        Vector3 restrictPlaneTangent = stepDir;
        restrictPlaneTangent.y = 0f;
        restrictPlaneTangent.Normalize();

        Vector3 projectedVelocity = Vector3.Dot(restrictPlaneTangent, velocity) * restrictPlaneTangent;
        projectedVelocity.y = 0f;
        pathVelocity.x = Mathf.Sign(pathVelocity.x) * projectedVelocity.magnitude;
        pathVelocity.y = cachedY;

        var rawNormal = motor.GroundingStatus.GroundNormal;
        projectedNormal = pathTransform.ProjectVectorOntoPlane(rawNormal).normalized;
    }

    public void PostGroundingUpdate(float deltaTime)
    {
        
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
}
