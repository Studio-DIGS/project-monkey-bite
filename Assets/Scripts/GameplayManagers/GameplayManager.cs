using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private SceneUnloadEventChannelSO askUnloadGameplayLevels;

    [ColorHeader("Listening - On Level Completed Channel", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private VoidEventChannelSO onLevelCompleted;
    
    [ColorHeader("Listening - Start new Run Ask Channel", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private VoidEventChannelSO askStartNewRun;
    
    [ColorHeader("Listening - Return To Main Menu Ask Channel", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private VoidEventChannelSO askReturnToMainMenu;
    
#if UNITY_EDITOR
    [ColorHeader("Reading - Cold Startup State", ColorHeaderColor.ReadingState)]
    [SerializeField] private ColdStartupDataSO coldStartupState;
#endif

    [ColorHeader("Dependencies", ColorHeaderColor.Dependencies)] 
    [SerializeField] private WorldGenerationProviderSO worldGenerationProvider;
    [SerializeField] private GameSceneSO runCompletedScene;

    // Internal state
    private bool levelLoaded = false;
    private WorldGenerationStrategy worldGenerationStrategyInstance;

    private void OnEnable()
    {
        // Disable input while loading
        askInputStateChange.RaiseEvent(InputState.Disabled);
        
        onGameplayManagerSceneLoaded.OnRaised += SetupGameplayManager;
        onLevelSceneLoaded.OnRaised += OnLevelSceneLoaded;
        onLevelCompleted.OnRaised += LoadNextLevel;
        askReturnToMainMenu.OnRaised += ReturnToMainMenu;
        askStartNewRun.OnRaised += StartNewRun;
    }

    private void OnDisable()
    {
        onGameplayManagerSceneLoaded.OnRaised -= SetupGameplayManager;
        onLevelSceneLoaded.OnRaised -= OnLevelSceneLoaded;
        onLevelCompleted.OnRaised -= LoadNextLevel;
        askReturnToMainMenu.OnRaised -= ReturnToMainMenu;
        askStartNewRun.OnRaised -= StartNewRun;
    }
    
    private void SetupGameplayManager()
    {

#if UNITY_EDITOR
        if (coldStartupState.isColdStartup)
        {
            askLoadGameplayLevel.RaiseEvent(coldStartupState.startupScene, false, true);
            coldStartupState.ConsumeColdStartup();
            return;
        }
#endif
        StartNewRun();
    }
    
    
    private void OnLevelSceneLoaded()
    {
        // Any extra work done before the player is spawned goes here
        levelLoaded = true;
        
        // Raise the event indicating the level is ready
        // The level scene handles the rest
        onLevelSceneReady.RaiseEvent();
    }


    private void LoadNextLevel()
    {
        // Disable input while loading
        askInputStateChange.RaiseEvent(InputState.Disabled);
        
        var nextLevel = worldGenerationStrategyInstance.GetNextLevel();

        // No more levels; Run has ended
        if (nextLevel == null)
        {
            EndRun();
        }
        else
        {
            // Load the next level
            Debug.Log("Request level");
            askLoadGameplayLevel.RaiseEvent(nextLevel, true, true);
        }
    }

    // Run ended, Load run finish scene
    private void EndRun()
    {
        askLoadGameplayLevel.RaiseEvent(runCompletedScene, true, true);
    }
    
    private void StartNewRun()
    {
        SetupWorldGeneration();
        LoadNextLevel();
    }
    
    private void SetupWorldGeneration()
    {
        worldGenerationStrategyInstance = worldGenerationProvider.CreateWorldGenerationStrategy();
        // TODO: Load save data into generation strategy
    }
    
    private bool escaped = false;

    private void Update()
    {
        // Simple debugging for exiting
        if (Input.GetKey(KeyCode.Escape) && !escaped && levelLoaded)
        {
            escaped = true;
            askReturnToMainMenu.RaiseEvent();
        }
    }

    private void ReturnToMainMenu()
    {
        // Save any data before leaving
        askInputStateChange.RaiseEvent(InputState.Disabled);
        askGameStateChange.RaiseEvent(GameState.MainMenu);
    }
}
