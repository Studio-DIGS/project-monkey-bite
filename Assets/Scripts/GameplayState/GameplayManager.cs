using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [ColorHeader("Listening - On Scene Load Channels", ColorHeaderColor.ListeningEvents)]
    [SerializeField] private VoidEventChannelSO onGameplayManagerSceneLoaded;
    [SerializeField] private VoidEventChannelSO onLevelSceneLoaded;

    [ColorHeader("Invoking - On Level Ready Channel", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private VoidEventChannelSO onLevelSceneReady;
    
    [ColorHeader("Invoking - Ask Input State Change Channel", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private InputStateEventChannelSO askInputStateChange;

    [ColorHeader("Invoking - Ask Game State Change Channel", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private GameStateEventChannelSO askGameStateChange;
    
    [ColorHeader("Invoking - Ask Scene Management Channels", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private SceneLoadEventChannelSO askLoadGameplayLevel;
    [SerializeField] private SceneUnloadAllEventChannelSO askUnloadGameplayLevels;
    
#if UNITY_EDITOR
    [ColorHeader("Reading - Cold Startup State", ColorHeaderColor.ReadingState)]
    [SerializeField] private ColdStartupDataSO coldStartupState;
#endif

    [ColorHeader("Gameplay Entry Scene", ColorHeaderColor.Dependencies)]
    [SerializeField] private GameSceneSO gameplayEntryScene;

    // Internal state
    private bool levelLoaded = false;

    private void OnEnable()
    {
        onGameplayManagerSceneLoaded.OnRaised += SetupGameplayManager;
        onLevelSceneLoaded.OnRaised += OnLevelSceneLoaded;
    }

    private void OnDisable()
    {
        onGameplayManagerSceneLoaded.OnRaised -= SetupGameplayManager;
        onLevelSceneLoaded.OnRaised -= OnLevelSceneLoaded;
    }

    private void SetupGameplayManager()
    {
        // Disable input
        askInputStateChange.RaiseEvent(InputState.Disabled);
        
        // Determine the entry scene, considering cold startups
        GameSceneSO entryScene = gameplayEntryScene;
        
#if UNITY_EDITOR
        if (coldStartupState.isColdStartup)
        {
            entryScene = coldStartupState.startupScene;
            coldStartupState.ConsumeColdStartup();
        }
#endif
        
        // Load the entry scene
        askLoadGameplayLevel.RaiseEvent(entryScene);
    }

    private void OnLevelSceneLoaded()
    {
        // Any extra work done before the player is spawned goes here
        levelLoaded = true;
        
        // Raise the event indicating the level is ready
        // The level scene handles the rest
        onLevelSceneReady.RaiseEvent();
    }

    private bool escaped = false;

    private void Update()
    {
        // Simple debugging for exiting
        if (Input.GetKey(KeyCode.Escape) && !escaped && levelLoaded)
        {
            escaped = true;
            ReturnToMainMenu();
        }
    }

    private void ReturnToMainMenu()
    {
        askUnloadGameplayLevels.RaiseEvent();
        askGameStateChange.RaiseEvent(GameState.MainMenu);
    }
}
