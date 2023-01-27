using System;
using UnityEngine;

/// <summary>
/// Simple controller for basic movement
/// </summary>
public class SimplePathMovement : MonoBehaviour
{
    [ColorHeader("Dependencies")]
    [SerializeField] private SplinePathPhysicsBody pathBody;

    
    public void SimpleHorizontalMovement(
        float input, 
        float maxVel, 
        float moveAccel,
        float frictionAccel,
        float timeStep, 
        Vector3 normal)
    {
        if (input == 0)
        {
            ApplyHorizontalFriction(frictionAccel, timeStep, normal);
        }
        else
        {
            HorizontalMove(input, maxVel, moveAccel, timeStep, normal);
        }
    }

    public void HorizontalMove(float input, float maxVel, float accel, float timeStep, Vector3 normal)
    {
        Vector2 cVel = pathBody.pathVelocity;
        float targetVel = input * maxVel;
        pathBody.pathVelocity.x = Mathf.MoveTowards(cVel.x, targetVel, accel * timeStep);
    }

    public void ApplyHorizontalFriction(float accel, float timeStep, Vector3 normal)
    {
        Vector2 cVel = pathBody.pathVelocity;
        pathBody.pathVelocity.x = Mathf.MoveTowards(cVel.x, 0f, accel * timeStep);
    }
}
