using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[RequireComponent (typeof (BoxCollider))]
public class PathController : MonoBehaviour {

    // Member variables
    // ----------------------------------------------------------------------------
    // protected component references
    PathCreator pathCreator;
    BoxCollider col;

    // public constants
    const float SKIN_WIDTH = 0.015f;

    // public inspector fields
    public bool startOnPath = true;
    public LayerMask collisionMask;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    // private fields
    private bool _onPath;
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
        // Path stuff
        pathCreator = GameObject.FindWithTag("WorldPath").GetComponent<PathCreator>();
        this.PutOnPath();

        // Collision Stuff
        col = GetComponent<BoxCollider>();
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
        
        _raycastOrigins.bottomLeft = (transform.forward * -1 * boundingSize.z) + bounds.center - new Vector3(0f, boundingSize.y, 0f);
        _raycastOrigins.bottomRight = (transform.forward * boundingSize.z) + bounds.center - new Vector3(0f, boundingSize.y, 0f);
        _raycastOrigins.topLeft = (transform.forward * -1 * boundingSize.z) + bounds.center + new Vector3(0f, boundingSize.y, 0f);
        _raycastOrigins.topRight = (transform.forward * boundingSize.z) + bounds.center + new Vector3(0f, boundingSize.y, 0f);
    }

    // Path interface
    // ----------------------------------------------------------------------------
    public void PutOnPath() { 
        _onPath = true;
        _distance = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        _height = transform.position.y;
        _onPath = startOnPath;
    }

    public void TakeOffPath() {
        _onPath = false;
    }

    // Movement interface
    // ----------------------------------------------------------------------------
    public void Move(Vector2 movement) {
        UpdateRaycastOrigins();
        if (movement.x != 0) 
            HorizontalCollision(ref movement);
        if (movement.y != 0) 
            VerticalCollision(ref movement);


        // Final movement along curve
        if (pathCreator != null && _onPath) {
            _distance += movement.x;
            _height += movement.y;

            Vector3 pathPos = pathCreator.path.GetPointAtDistance(_distance);
            transform.position = new Vector3(pathPos.x, _height, pathPos.z);
            transform.forward = pathCreator.path.GetDirectionAtDistance(_distance);
        }
    }

    // Movement helper functions
    // ----------------------------------------------------------------------------
    void VerticalCollision(ref Vector2 movement) {
        float directionY = Mathf.Sign(movement.y); // moving down = -1, moving up = +1
        float rayLength = Mathf.Abs(movement.y) + SKIN_WIDTH; 
        
        for (int i = 0; i < verticalRayCount; i++) {
            // set origin of ray to either bottom or top of character depending on direction of movement
            Vector3 rayOrigin = (directionY == -1)?_raycastOrigins.bottomLeft:_raycastOrigins.topLeft;
            rayOrigin += transform.forward * (_verticalRaySpacing * i + movement.x);
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, Vector3.up * directionY, out hit, rayLength, collisionMask)) {
                movement.y = (hit.distance - SKIN_WIDTH) * directionY;
                rayLength = hit.distance; // important so that there aren't conflicting hits from the different raycasts
            }  

            Debug.DrawRay(rayOrigin, Vector3.up * directionY * rayLength, Color.red);
        }
    }

    void HorizontalCollision(ref Vector2 movement) {
        float directionX = Mathf.Sign(movement.x); // moving left = -1, moving right = +1
        float rayLength = Mathf.Abs(movement.x) + SKIN_WIDTH; 
        
        for (int i = 0; i < horizontalRayCount; i++) {
            // set origin of ray to either left or right of character depending on direction of movement
            Vector3 rayOrigin = (directionX == -1)?_raycastOrigins.bottomLeft:_raycastOrigins.bottomRight;
            rayOrigin += Vector3.up * (_horizontalRaySpacing * i);
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, transform.forward * directionX, out hit, rayLength, collisionMask)) {
                movement.x = (hit.distance - SKIN_WIDTH) * directionX;
                rayLength = hit.distance; // important so that there aren't conflicting hits from the different raycasts
            }  

            Debug.DrawRay(rayOrigin, transform.forward * directionX * rayLength, Color.blue);
        }
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

    //     float angle = Vector3.Angle(transform.forward, delta);
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
