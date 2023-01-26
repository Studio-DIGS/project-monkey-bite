using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gameplay Level Scene", menuName = "SceneCreation/Scene Data SO/Gameplay Level")]
public class GameplayLevelSceneSO : GameSceneSO
{
    [SerializeField] private string levelDisplayName;
    public string LevelDisplayName => levelDisplayName;
}
