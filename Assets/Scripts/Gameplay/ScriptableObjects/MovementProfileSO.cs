using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameplayProfiles/Movement Profile", fileName = "MovementProfile")]
public class MovementProfileSO : DescriptionBaseSO
{
    public float moveSpeed;
    public float acceleration;
    public float gravity;
    public float jumpStrength;
    public float minJumpTime;
    public float maxJumpTime;
}
