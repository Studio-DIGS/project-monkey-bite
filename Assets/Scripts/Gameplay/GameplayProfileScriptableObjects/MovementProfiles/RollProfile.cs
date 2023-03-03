using UnityEngine;

[CreateAssetMenu(menuName = "GameplayProfiles/Movement/Roll Profile", fileName = "RollProfile")]
public class RollProfile  : DescriptionBaseSO
{
    public MotionCurve rollMotionCurve;
    public Vector2 exitVelocity;
}
