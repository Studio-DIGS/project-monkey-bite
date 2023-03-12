using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
#if UNITY_EDITOR
    private Vector3 groundPos;
    private LayerMask castMask;
    
    private void OnDrawGizmos()
    {
        if(castMask == default)
            castMask = LayerMask.GetMask("Environment");
        if (Physics.Raycast(new Ray(transform.position, Vector3.down), out RaycastHit hitInfo, castMask))
        {
            groundPos = hitInfo.point;
        }
        else
        {
            groundPos = transform.position + Vector3.down * 100f;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position,groundPos);
        Gizmos.DrawSphere(transform.position, 0.5f);
        Gizmos.DrawSphere(groundPos, 0.2f);
    }
#endif
}
