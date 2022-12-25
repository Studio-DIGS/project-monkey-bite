using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleGenerationStrategy : WorldGenerationStrategy
{
    private GameplayLevelSceneSO finalLevel;
    private List<GameplayLevelGroupSO> gameplayLevelGroups;

    private int levelsPerGroup;
    private int groups;

    private IEnumerator levelEnumerator;
    
    public SimpleGenerationStrategy(
        GameplayLevelSceneSO finalLevel, 
        List<GameplayLevelGroupSO> gameplayLevelGroups,
        int levelsPerGroup,
        int groups)
    {
        this.finalLevel = finalLevel;
        this.gameplayLevelGroups = new List<GameplayLevelGroupSO>(gameplayLevelGroups);
        this.levelsPerGroup = levelsPerGroup;
        this.groups = groups;
        
        levelEnumerator = LevelEnumerator().GetEnumerator();
    }
    
    public GameplayLevelSceneSO GetNextLevel()
    {
        levelEnumerator.MoveNext();
        var current = (GameplayLevelSceneSO)levelEnumerator.Current;
        return current;
    }

    private IEnumerable LevelEnumerator()
    {
        for (int i = 0; i < groups; i++)
        {
            // Select a random level group
            int selectedGroupIndex = Random.Range(0,gameplayLevelGroups.Count);
            var selectedGroup = gameplayLevelGroups[selectedGroupIndex];
            gameplayLevelGroups.RemoveAt(selectedGroupIndex);
            
            // Select random levels from the chosen group
            var levels = new List<GameplayLevelSceneSO>(selectedGroup.LevelSceneGroup);
            for (int j = 0; j < levelsPerGroup - 1; j++)
            {
                int selectedLevelIndex = Random.Range(0,levels.Count);
                var selectedLevel = levels[selectedLevelIndex];
                levels.RemoveAt(selectedLevelIndex);
                yield return selectedLevel;
            }

            // Select a random final level from the group
            var finalLevels = selectedGroup.FinalLevelSceneGroup;
            yield return finalLevels[Random.Range(0, finalLevels.Count)];
        }
        
        yield return finalLevel;

        yield return null;
    }

    public void LoadGenerationData(string serializedData)
    {
        throw new System.NotImplementedException();
    }

    public void SaveGenerationData()
    {
        throw new System.NotImplementedException();
    }
}
