using System;
using MushiCore.EditorAttributes;
using UnityEngine;

/// <summary>
/// Simple controller for basic movement
/// </summary>
public class SimplePathMovement : MonoBehaviour
{
    private PathControllerMotor controllerMotor;

    public void Initialize(PathControllerMotor controllerMotor)
    {
        this.controllerMotor = controllerMotor;
    }

    public void SimpleGroundedHorizontalMovement(
        float input, 
        float maxVel, 
        float moveAccel,
        float frictionAccel,
        float timeStep, 
        Vector2 normal)
    {
        if (input == 0)
        {
            var step = CalculateHorizontalFrictionStep(frictionAccel, timeStep, normal);
            controllerMotor.pathVelocity += step;
        }
        else
        {
            controllerMotor.pathVelocity += CalculateHorizontalStep(input, maxVel, moveAccel, timeStep, normal);
        }
    }
    
    public void SimpleAirborneHorizontalMovement(
        float input, 
        float maxVel, 
        float moveAccel,
        float frictionAccel,
        float timeStep,
        Vector2 normal)
    {
        Vector2 step = Vector2.zero;
        if (controllerMotor.currentGroundState.isTouchingGround)
        {
            if (input * controllerMotor.currentGroundState.groundNormal.x <= 0f)
            {
                input = 0;
            }
        }
        if (input == 0)
        {
            step = CalculateHorizontalFrictionStep(frictionAccel, timeStep, normal);
        }
        else
        {
            step = CalculateHorizontalStep(input, maxVel, moveAccel, timeStep, normal);
        }
        controllerMotor.pathVelocity += step;
    }

    public Vector2 CalculateHorizontalStep(float input, float maxVel, float accel, float timeStep, Vector2 normal)
    {
        // Current velocity projected onto normal plane
        Vector2 cVel = controllerMotor.pathVelocity;
        Vector2 cVelNormalProject = Vector2.Dot(cVel, normal) * normal;
        Vector2 cHVel = cVel - cVelNormalProject;

        // Target velocity on normal plane
        Vector2 targetVel = Vector2.right * input;
        Vector2 targetVelNormalProject = Vector2.Dot(targetVel, normal) * normal;
        Vector2 targetHVel = (targetVel - targetVelNormalProject).normalized * maxVel;
        
        // Step towards target velocity
        Vector2 newVel = Vector2.MoveTowards(cHVel, targetHVel, accel * timeStep);
        Vector2 step = newVel - cHVel;
        return step;
    }

    public Vector2 CalculateHorizontalFrictionStep(float accel, float timeStep, Vector2 normal)
    {
        return CalculateHorizontalStep(0f, 1000f, accel, timeStep, normal);
    }
}
