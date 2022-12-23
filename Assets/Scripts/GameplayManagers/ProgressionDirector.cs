using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionDirector : MonoBehaviour
{
    [ColorHeader("Listening - On Level Loaded Channels", ColorHeaderColor.ListeningEvents)]
    [SerializeField] private VoidEventChannelSO onLevelSceneLoaded;

    [ColorHeader("Invoking - On Level Ready Channel", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private VoidEventChannelSO onLevelSceneReady;
    
    [ColorHeader("Invoking - Ask Input State Change Channel", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private InputStateEventChannelSO askInputStateChange;

    [ColorHeader("Invoking - Ask Game State Change Channel", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private GameStateEventChannelSO askGameStateChange;
    
    [ColorHeader("Invoking - Ask Scene Management Channels", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private SceneLoadEventChannelSO askLoadGameplayLevel;
    [SerializeField] private SceneUnloadEventChannelSO askUnloadGameplayLevels;
        
#if UNITY_EDITOR
    [ColorHeader("Reading - Cold Startup State", ColorHeaderColor.ReadingState)]
    [SerializeField] private ColdStartupDataSO coldStartupState;
#endif

    [ColorHeader("Dependencies", ColorHeaderColor.Dependencies)] 
    [SerializeField] private WorldGenerationProviderSO worldGenerationProvider;

    // Internal state
    private bool levelLoaded = false;
    private WorldGenerationStrategy worldGenerationStrategyInstance;
    
    
}
