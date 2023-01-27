using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Unity.Mathematics;
using UnityEngine.Splines;

public class PathController : MonoBehaviour {

    // Member variables
    // ----------------------------------------------------------------------------
    // protected component references
    [SerializeField] private BoxCollider col;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private SplinePathPhysicsBody splineBody;

    // public constants
    const float SKIN_WIDTH = 0.015f;

    // public inspector fields
    // [Header ("Movement Settings")]
    [Header ("Path Settings")]
    public bool startOnPath = true;
    [Header ("Collision Settings")]
    public LayerMask collisionMask;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    
    private bool _onPath;
    private bool _shouldJump;
    private float _distance, _height;
    private float _horizontalRaySpacing;
    private float _verticalRaySpacing;
    private RaycastOrigins _raycastOrigins;

    // Structs
    // ----------------------------------------------------------------------------
    public struct RaycastOrigins {
        public Vector3 topLeft, topRight, bottomLeft, bottomRight;
    }

    // Callbacks
    // ----------------------------------------------------------------------------
    void OnEnable() {
        // Collision Stuff
        CalculateRaySpacing();
    }

    // Unity lifetime functions
    // ----------------------------------------------------------------------------
    void FixedUpdate() {
        // Handle collision code here
        return;
    }

    void Reset() { 
        // Give default values to box collider when added in the inspector
        col = GetComponent<BoxCollider>();
        col.size = new Vector3 (0f, col.size.y, col.size.z);
    }

    // Accessor functions
    // ----------------------------------------------------------------------------
    public RaycastOrigins getRaycastOrigins() {
        return _raycastOrigins;
    }

    // Raycast interface
    // ----------------------------------------------------------------------------
    public void UpdateRaycastOrigins() {
        Bounds bounds = col.bounds;
        Vector3 boundingSize = (col.size - (new Vector3(0,1,1) * SKIN_WIDTH)) / 2;
        
        _raycastOrigins.bottomLeft = (targetTransform.forward * -1 * boundingSize.z) + bounds.center - new Vector3(0f, boundingSize.y, 0f);
        _raycastOrigins.bottomRight = (targetTransform.forward * boundingSize.z) + bounds.center - new Vector3(0f, boundingSize.y, 0f);
        _raycastOrigins.topLeft = (targetTransform.forward * -1 * boundingSize.z) + bounds.center + new Vector3(0f, boundingSize.y, 0f);
        _raycastOrigins.topRight = (targetTransform.forward * boundingSize.z) + bounds.center + new Vector3(0f, boundingSize.y, 0f);
    }

    // Path interface
    // ----------------------------------------------------------------------------

    public void TakeOffPath() {
        _onPath = false;
    }

    // Movement interface
    // ----------------------------------------------------------------------------
    public bool OnGround() {
        Vector2 movement = new Vector2(0,0.01f);
        return VerticalCollision(ref movement, false);
    }

    public Vector2 Move(Vector2 velocity) {
        UpdateRaycastOrigins();
        if (velocity.x != 0)
            HorizontalCollision(ref velocity, velocity.x > 0);
         if (velocity.y != 0)
            VerticalCollision(ref velocity, velocity.y > 0);

         splineBody.velocity.x = velocity.x;

         return velocity;
    }

    // Movement helper functions
    // ----------------------------------------------------------------------------
    bool VerticalCollision(ref Vector2 movement, bool raycastUp) {
        float directionY = raycastUp ? 1 : -1; // moving down = -1, moving up = +1
        float rayLength = Mathf.Abs(movement.y) + SKIN_WIDTH; 
        
        bool foundCollision = false;
        for (int i = 0; i < verticalRayCount; i++) {
            // set origin of ray to either bottom or top of character depending on direction of movement
            Vector3 rayOrigin = (directionY == -1)?_raycastOrigins.bottomLeft:_raycastOrigins.topLeft;
            rayOrigin += targetTransform.forward * (_verticalRaySpacing * i + movement.x);
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, Vector3.up * directionY, out hit, rayLength, collisionMask)) {
                movement.y = (hit.distance - SKIN_WIDTH) * directionY;
                rayLength = hit.distance; // important so that there aren't conflicting hits from the different raycasts
                foundCollision = true;
            }

            Debug.DrawRay(rayOrigin, Vector3.up * directionY * rayLength, Color.red);
        }
        return foundCollision;
    }

    bool HorizontalCollision(ref Vector2 movement, bool raycastRight) {
        float directionX = raycastRight ? 1 : -1; // moving left = -1, moving right = +1
        float rayLength = Mathf.Abs(movement.x) + SKIN_WIDTH; 
        
        bool foundCollision = false;
        for (int i = 0; i < horizontalRayCount; i++) {
            // set origin of ray to either left or right of character depending on direction of movement
            Vector3 rayOrigin = (directionX == -1)?_raycastOrigins.bottomLeft:_raycastOrigins.bottomRight;
            rayOrigin += Vector3.up * (_horizontalRaySpacing * i);
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, targetTransform.forward * directionX, out hit, rayLength, collisionMask)) {
                movement.x = (hit.distance - SKIN_WIDTH) * directionX;
                rayLength = hit.distance; // important so that there aren't conflicting hits from the different raycasts
                foundCollision = true;
            }  

            Debug.DrawRay(rayOrigin, targetTransform.forward * directionX * rayLength, Color.blue);
        }
        return foundCollision;
    }

    void CalculateRaySpacing() {
        Vector3 boundingSize = col.size - (new Vector3(0,1,1) * SKIN_WIDTH);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        _horizontalRaySpacing = (boundingSize.y) / (horizontalRayCount - 1);
        _verticalRaySpacing = (boundingSize.z) / (verticalRayCount - 1);
    }

    // ADD THE BELOW CODE LATER //

    // public Vector2 GetTargetVector(Vector3 objectPos, float offset)
    // {
    //     Vector2 targetVector;
    //     Vector3 virtualPathPos = pathCreator.path.GetPointAtDistance(distance + offset);
    //     Vector3 virtualPos = new Vector3(virtualPathPos.x, height, virtualPathPos.z);
    //     Vector3 virtualTangent = pathCreator.path.GetDirectionAtDistance(distance + offset);
    //     Vector3 delta = objectPos - virtualPos;

    //     float angle = Vector3.Angle(targetTransform.forward, delta);
    //     if (angle <= 90f)
    //     {
    //         targetVector.x = 1;
    //     }
    //     else
    //     {
    //         targetVector.x = -1;
    //     }

    //     targetVector.y = Mathf.Clamp(delta.y, -1, 1);

    //     targetVector *= Mathf.Clamp(delta.sqrMagnitude, 0, 1);
    //     return targetVector;
    // }
}
