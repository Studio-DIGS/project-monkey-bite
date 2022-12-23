using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/Gameplay/Level Generation/Simple Generation Provider")]
public class SimpleGenerationStrategyProvider : WorldGenerationProviderSO
{
    [SerializeField] private GameplayLevelSceneSO entryLevel;
    [SerializeField] private List<GameplayLevelGroupSO> levelGroups;
    [SerializeField] private GameplayLevelSceneSO finalLevel;
    
    public override WorldGenerationStrategy CreateWorldGenerationStrategy()
    {
        return new SimpleGenerationStrategy(
            entryLevel, 
            finalLevel,
            levelGroups,
            2,
            1);
    }
}
