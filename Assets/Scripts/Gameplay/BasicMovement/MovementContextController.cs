using System;
using MushiCore.EditorAttributes;
using UnityEngine;

/// <summary>
/// Handles movement context things like grounded checks, grounded normals, slope detection, etc.
/// Works in the context of spline space, yay
/// </summary>
public class MovementContextController : DescriptionMonoBehavior
{
    [ColorHeader("Config", ColorHeaderColor.Config)]
    [SerializeField] private SplinePathPhysicsBody pathBody;
    [SerializeField] private Transform raycastSource;
    [SerializeField] private float castRadius;
    [SerializeField] private float castDistance;
    [SerializeField] private LayerMask groundedMask;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float groundedDotMin;

    // Fields
    private bool isGrounded;
    private bool isOnSurface;
    private bool isOnEnemy;
    private Vector2 surfaceNormal;
    
    // Properties
    public bool IsGrounded => isGrounded;
    public bool IsOnSurface => isOnSurface;
    public bool IsOnEnemy => isOnEnemy;
    public Vector2 SurfaceNormal => surfaceNormal;
    
    
    /// <summary>
    /// Recalculates context
    /// </summary>
    public void UpdateContext()
    {
        CheckGrounded();
        CheckOnEnemy();
    }

    private void CheckGrounded()
    {
        Vector3 pos = raycastSource.position;
        bool didHit = Physics.SphereCast(
            pos, 
            castRadius, 
            Vector3.down, 
            out RaycastHit hitInfo, 
            castDistance, 
            groundedMask);

        if (didHit)
        {
            isOnSurface = true;
            surfaceNormal = pathBody.ProjectVecOntoPath(hitInfo.normal).normalized;
            // Check the dot to make sure the slope isnt too steep
            float dot = Vector2.Dot(Vector2.up, surfaceNormal);
            if (dot > groundedDotMin)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            surfaceNormal = Vector2.up;
            isOnSurface = false;
            isGrounded = false;
        }
    }

    private void CheckOnEnemy()
    {
        Vector3 pos = raycastSource.position;
        bool didHit = Physics.SphereCast(
            pos, 
            castRadius, 
            Vector3.down, 
            out RaycastHit hitInfo, 
            castDistance, 
            enemyMask);

        if (didHit)
        {
            isOnSurface = true;
            surfaceNormal = pathBody.ProjectVecOntoPath(hitInfo.normal).normalized;
            // Check the dot to make sure the slope isnt too steep
            float dot = Vector2.Dot(Vector2.up, surfaceNormal);
            if (dot > groundedDotMin)
            {
                isOnEnemy = true;
            }
            else
            {
                isOnEnemy = false;
            }
        }
        else
        {
            surfaceNormal = Vector2.up;
            isOnEnemy = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 pos = raycastSource.position;
        Vector3 lowerPos = pos + castDistance * Vector3.down;
        Gizmos.DrawLine(pos, lowerPos);
        Gizmos.DrawWireSphere(pos, castRadius);
        Gizmos.DrawWireSphere(lowerPos, castRadius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 pos2 = raycastSource.position - castDistance * Vector3.down;
        Vector3 normal = pathBody.PathToWorldVec(surfaceNormal).normalized;
        Gizmos.DrawLine(pos2, pos2 + normal * 3f);
    }
}
