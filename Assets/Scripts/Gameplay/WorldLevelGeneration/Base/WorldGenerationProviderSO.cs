using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Provides a world generation strategy based on configuration
/// </summary>
public abstract class WorldGenerationProviderSO : DescriptionBaseSO
{
    public abstract WorldGenerationStrategy CreateWorldGenerationStrategy();
}
