using System;
using UnityEngine;

/// <summary>
/// Handles movement context things like grounded checks, grounded normals, slope detection, etc.
/// </summary>
public class MovementContextController : DescriptionMonoBehavior
{
    [ColorHeader("Config", ColorHeaderColor.Config)]
    [SerializeField] private Transform raycastSource;
    [SerializeField] private float castRadius;
    [SerializeField] private float castDistance;
    [SerializeField] private LayerMask groundedMask;
    [SerializeField] private float groundedDotThreshhold;

    // Fields
    private bool isGrounded;
    private Vector3 groundedNormal;
    
    // Properties
    public bool IsGrounded => isGrounded;
    public Vector3 GroundedNormal => groundedNormal;
    
    
    /// <summary>
    /// Recalculates context
    /// </summary>
    public void UpdateContext()
    {
        CheckGrounded();
        if (!isGrounded)
        {
            groundedNormal = Vector3.up;
        }
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
            isGrounded = true;
            groundedNormal = hitInfo.normal;
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
}
