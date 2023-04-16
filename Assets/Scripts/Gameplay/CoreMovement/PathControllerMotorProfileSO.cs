using MushiCore.EditorAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "GameplayProfiles/Physics/PCCMotorProfile", fileName = "PCCMotorProfile")]
public class PathControllerMotorProfileSO : DescriptionBaseSO
{
    [ColorHeader("Config", ColorHeaderColor.Config)]
    [SerializeField] private float gravityAcceleration;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float maxMoveIterationLength;
    [SerializeField, Range(1, 10)] private int minMoveIterations;
    
    [ColorHeader("Collision Resolution Config", ColorHeaderColor.Config)]
    [SerializeField] private float collisionResolutionOffset;
    [SerializeField] private float sweepOffset;
    [SerializeField] private int maxColliderResolves;

    [ColorHeader("Grounding Config", ColorHeaderColor.Config)]
    [SerializeField] private float groundProbeDistance;
    [SerializeField] private float groundTouchDistance;
    [SerializeField] private float maxStableAngle;
    [SerializeField] private float launchOffLedgeVelocity;
    [SerializeField] private int maxGroundProbeIterations;
    
    public float GravityAcceleration => gravityAcceleration;
    public LayerMask CollisionMask => collisionMask;
    public float MaxMoveIterationLength => maxMoveIterationLength;
    public int MinMoveIterations => minMoveIterations;

    public float CollisionResolutionOffset => collisionResolutionOffset;
    public float SweepOffset => sweepOffset;
    public int MaxColliderResolves => maxColliderResolves;
    
    public float GroundProbeDistance => groundProbeDistance;
    public float GroundTouchDistance => groundTouchDistance;
    public float MaxStableAngle => maxStableAngle;
    public float LaunchOffLedgeVelocity => launchOffLedgeVelocity;
    public int MaxGroundProbeIterations => maxGroundProbeIterations;
}
