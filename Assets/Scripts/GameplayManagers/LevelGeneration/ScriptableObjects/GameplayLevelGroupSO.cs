using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/Gameplay/Level Generation/LevelGroup")]
public class GameplayLevelGroupSO : ScriptableObject
{
    [ColorHeader("List of normal level scenes in the group")]
    [SerializeField] private List<GameplayLevelSceneSO> levelSceneGroup;

    public List<GameplayLevelSceneSO> LevelSceneGroup => levelSceneGroup;

    [ColorHeader("List of final level scenes in the group")]
    [SerializeField] private List<GameplayLevelSceneSO> finalLevelSceneGroup;

    public List<GameplayLevelSceneSO> FinalLevelSceneGroup => finalLevelSceneGroup;
}
