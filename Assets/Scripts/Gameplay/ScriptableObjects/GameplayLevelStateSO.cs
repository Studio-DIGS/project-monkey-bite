using UnityEngine;
using UnityEngine.Splines;

[CreateAssetMenu(menuName = "Architecture/Gameplay/GameplayLevelStateSO", fileName = "GameplayLevelStateSO")]
public class GameplayLevelStateSO : DescriptionBaseSO
{
    [ReadOnly, SerializeField] public SplineContainer levelCurve;
}
