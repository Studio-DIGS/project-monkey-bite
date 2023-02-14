using System.Collections;
using System.Collections.Generic;
using MushiCore.EditorAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "GameplayProfiles/Movement Profile", fileName = "MovementProfile")]
public class MovementProfileSO : DescriptionBaseSO
{
    [ColorHeader("Grounded Movement")]
    public float groundedWalkVel;
    public float groundedWalkAccel;
    public float groundedFriction;

    [ColorHeader("Airborne Movement")]
    public float airborneWalkVel;
    public float airborneWalkAccel;
    public float airborneFriction;
    
    [ColorHeader("Jump")]
    public float jumpStrength;
    public float jumpEndVel;
    public float coyoteTime;
    public float maxJumpTime;
    public float minJumpTime;

    [ColorHeader("Footstool Jump")]
    public LayerMask footstoolMask;
    public float ftstlJumpStrength;
    public float ftstlJumpEndVel;
    public float ftstlCoyoteTime;
    public float ftstlMaxJumpTime;
    public float ftstlMinJumpTime;
}
