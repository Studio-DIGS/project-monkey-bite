using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/Gameplay/LevelGroup")]
public class GameplayLevelGroupSO : ScriptableObject
{
    [SerializeField] private List<GameplayLevelSceneSO> levelSceneGroup;

    public List<GameplayLevelSceneSO> LevelSceneGroup => levelSceneGroup;
}
