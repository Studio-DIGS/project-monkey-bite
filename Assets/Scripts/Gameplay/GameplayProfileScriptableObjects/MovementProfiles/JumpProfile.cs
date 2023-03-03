using MushiCore.EditorAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "GameplayProfiles/Movement/Jump Profile", fileName = "JumpProfile")]
public class JumpProfile : DescriptionBaseSO
{
    [ColorHeader("Jump Profile")]
    public float jumpHeight;
    public MotionCurve jumpCurve;
    public float jumpEndVel;
    public float coyoteTime;
    public float minJumpTime;
}