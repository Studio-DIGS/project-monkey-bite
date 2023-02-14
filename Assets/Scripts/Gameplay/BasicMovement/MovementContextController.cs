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
    [SerializeField] private float groundedDotMin;

    // Fields
    private bool isGrounded;
    private bool isOnSurface;
    private Vector2 surfaceNormal;
    
    // Properties
    public bool IsGrounded => isGrounded;
    public bool IsOnSurface => isOnSurface;
    public Vector2 SurfaceNormal => surfaceNormal;

    public struct GroundedInfo
    {
        public bool surfaceFound;
        public Vector2 surfaceNormal;
        public float groundedDot;
    }
    
    
    /// <summary>
    /// Recalculates context
    /// </summary>
    public void UpdateContext()
    {
        UpdateGroundedState();
    }

    /// <summary>
    /// Checks if the player is grounded on a layer
    /// </summary>
    public GroundedInfo CheckGroundedOnLayer(LayerMask layer)
    {
        Vector3 pos = raycastSource.position;
        var returnInfo = new GroundedInfo{
            surfaceFound = false,
            surfaceNormal = Vector2.up,
            groundedDot = 0f
        };
        bool didHit = Physics.SphereCast(
            pos, 
            castRadius, 
            Vector3.down, 
            out RaycastHit hitInfo, 
            castDistance, 
            layer);

        if (didHit)
        {
            returnInfo.surfaceFound = true;
            returnInfo.surfaceNormal = pathBody.ProjectVecOntoPath(hitInfo.normal).normalized;
            // Check the dot to make sure the slope isnt too steep
            returnInfo.groundedDot = Vector2.Dot(Vector2.up, surfaceNormal);
            
        }
        else
        {
            returnInfo.surfaceNormal = Vector2.up;
            returnInfo.surfaceFound = false;
        }

        return returnInfo;
    }

    private void UpdateGroundedState()
    {
        var castResults = CheckGroundedOnLayer(groundedMask);
        isOnSurface = castResults.surfaceFound;
        surfaceNormal = castResults.surfaceNormal;
        if (castResults.groundedDot > groundedDotMin)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
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
