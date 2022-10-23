using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[RequireComponent (typeof (BoxCollider))]
public class PathController : MonoBehaviour
{
    private PathCreator pathCreator;
    public bool onPath = true;
    // public LayerMask layerMask;
    // private Vector2 targetDir;
    // private Vector3 pos3;
    [HideInInspector] public float distance, height;

    public LayerMask collisionMask;
    const float skinWidth = 0.015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    RaycastOrigins raycastOrigins;
    BoxCollider col;

    void OnEnable()
    {
        // Path stuff
        pathCreator = GameObject.FindWithTag("WorldPath").GetComponent<PathCreator>();
        distance = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        height = transform.position.y;        

        // Collision Stuff
        col = GetComponent<BoxCollider>();
        CalculateRaySpacing();
        
    }

    void Reset() // Give default values to box collider when added in the inspector
    {
        col = GetComponent<BoxCollider>();
        col.size = new Vector3 (0f, col.size.y, col.size.z);
    }

    private void FixedUpdate() 
    {
        // Handle collision code here
        return;
    }

    public void Move(Vector2 movement)
    {
        UpdateRaycastOrigins();
        if (movement.x != 0) {HorizontalCollision(ref movement);}
        if (movement.y != 0) {VerticalCollision(ref movement);}


        // Final movement along curve
        if (pathCreator !=null && onPath)
        {
            distance += movement.x; // * Time.deltaTime;
            height += movement.y; // * Time.deltaTime;

            Vector3 pathPos = pathCreator.path.GetPointAtDistance(distance);
            transform.position = new Vector3(pathPos.x, height, pathPos.z);
            transform.forward = pathCreator.path.GetDirectionAtDistance(distance);
        }
    }

    void VerticalCollision(ref Vector2 movement)
    {
        float directionY = Mathf.Sign(movement.y); // moving down = -1, moving up = +1
        float rayLength = Mathf.Abs(movement.y) + skinWidth; 
        
        for (int i = 0; i < verticalRayCount; i++)
        {
            // set origin of ray to either bottom or top of character depending on direction of movement
            Vector3 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
            rayOrigin += transform.forward * (verticalRaySpacing * i + movement.x);
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, Vector3.up * directionY, out hit, rayLength, collisionMask))
            {
                movement.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance; // important so that there aren't conflicting hits from the different raycasts
            }  

            Debug.DrawRay(rayOrigin, Vector3.up * directionY * rayLength, Color.red);
        }
    }

    void HorizontalCollision(ref Vector2 movement)
    {
        float directionX = Mathf.Sign(movement.x); // moving left = -1, moving right = +1
        float rayLength = Mathf.Abs(movement.x) + skinWidth; 
        
        for (int i = 0; i < horizontalRayCount; i++)
        {
            // set origin of ray to either left or right of character depending on direction of movement
            Vector3 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
            rayOrigin += Vector3.up * (horizontalRaySpacing * i);
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, transform.forward * directionX, out hit, rayLength, collisionMask))
            {
                movement.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance; // important so that there aren't conflicting hits from the different raycasts
            }  

            Debug.DrawRay(rayOrigin, transform.forward * directionX * rayLength, Color.blue);
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = col.bounds;
        Vector3 boundingSize = (col.size - (new Vector3(0,1,1) * skinWidth)) / 2;
        
        raycastOrigins.bottomLeft = (transform.forward * -1 * boundingSize.z) + bounds.center - new Vector3(0f, boundingSize.y, 0f);
        raycastOrigins.bottomRight = (transform.forward * boundingSize.z) + bounds.center - new Vector3(0f, boundingSize.y, 0f);
        raycastOrigins.topLeft = (transform.forward * -1 * boundingSize.z) + bounds.center + new Vector3(0f, boundingSize.y, 0f);
        raycastOrigins.topRight = (transform.forward * boundingSize.z) + bounds.center + new Vector3(0f, boundingSize.y, 0f);
    }

    void CalculateRaySpacing()
    {
        Vector3 boundingSize = col.size - (new Vector3(0,1,1) * skinWidth);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = (boundingSize.y) / (horizontalRayCount - 1);
        verticalRaySpacing = (boundingSize.z) / (verticalRayCount - 1);
    }

    struct RaycastOrigins
    {
        public Vector3 topLeft, topRight;
        public Vector3 bottomLeft, bottomRight;
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
