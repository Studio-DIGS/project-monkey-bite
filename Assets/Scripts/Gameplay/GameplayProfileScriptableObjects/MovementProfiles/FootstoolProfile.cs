using MushiCore.EditorAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "GameplayProfiles/Movement/Footstool Profile", fileName = "FootstoolProfile")]
public class FootstoolProfile : DescriptionBaseSO
{
    [ColorHeader("Footstool Profile")]
    public LayerMask footstoolMask;
    public float jumpHeight;
    public MotionCurve jumpCurve;
    public float jumpEndVerticalVel;
    public float jumpEndHorizontalVel;
    public float minJumpTime;
}
