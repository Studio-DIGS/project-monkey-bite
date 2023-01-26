using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class to represent a world generation provider.
/// Functions like an enumerator, except it returns the next gameplay level in the world generation
/// Needs to support save/load
/// </summary>
public interface WorldGenerationStrategy
{
    public abstract GameplayLevelSceneSO GetNextLevel();

    public abstract void LoadGenerationData(string serializedData);

    public abstract void SaveGenerationData();
}
