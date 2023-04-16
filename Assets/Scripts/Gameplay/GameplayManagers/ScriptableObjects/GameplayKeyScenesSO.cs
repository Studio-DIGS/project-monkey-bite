using System.Collections;
using System.Collections.Generic;
using MushiCore;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/Gameplay/Gameplay Key Scenes ScriptableObject")]
public class GameplayKeyScenesSO : DescriptionBaseSO
{
    [SerializeField] private GameSceneSO entryScene;
    [SerializeField] private GameSceneSO runFinishMenuScene;

    public GameSceneSO EntryScene => entryScene;
    public GameSceneSO RunFinishMenuScene => runFinishMenuScene;
}
