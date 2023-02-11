using MushiCore.EditorAttributes;
using UnityEngine;
using UnityEngine.Splines;

[CreateAssetMenu(menuName = "Architecture/Gameplay/GameplayLevelStateSO", fileName = "GameplayLevelStateSO")]
public class GameplayLevelStateSO : DescriptionBaseSO
{
    [EditorReadOnly, SerializeField] public SplineContainer levelPath;
}
