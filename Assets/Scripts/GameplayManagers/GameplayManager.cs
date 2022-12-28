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
    
    [ColorHeader("Invoking - Ask Save Data Channel", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private ProfileSaveDataEventChannelSO askSaveProfile;
    
#if UNITY_EDITOR
    [ColorHeader("Reading - Cold Startup State", ColorHeaderColor.ReadingState)]
    [SerializeField] private ColdStartupDataSO coldStartupState;
#endif

    [ColorHeader("Dependencies", ColorHeaderColor.Dependencies)] 
    [SerializeField] private WorldGenerationProviderSO worldGenerationProvider;
    [SerializeField] private GameplayKeyScenesSO gameplayKeyScenes;
    [SerializeField] private ProfileSaveDataSO activeSaveContainer;

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
        askStartNewRun.OnRaised += EnterRunLobby;
    }

    private void OnDisable()
    {
        onGameplayManagerSceneLoaded.OnRaised -= SetupGameplayManager;
        onLevelSceneLoaded.OnRaised -= OnLevelSceneLoaded;
        onLevelCompleted.OnRaised -= LoadNextLevel;
        askReturnToMainMenu.OnRaised -= ReturnToMainMenu;
        askStartNewRun.OnRaised -= EnterRunLobby;
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
        EnterRunLobbyFromMenu();
    }
    
    
    private void OnLevelSceneLoaded()
    {
        // Any extra work done before the player is spawned goes here
        levelLoaded = true;
        
        // Raise the event indicating the level is ready
        // The level scene handles the rest
        onLevelSceneReady.RaiseEvent();
    }

    private void LoadEntryLevel(bool transitionOut)
    {
        LoadLevel(gameplayKeyScenes.EntryScene, transitionOut, true);
    }

    private void LoadNextLevel()
    {
        var nextLevel = worldGenerationStrategyInstance.GetNextLevel();

        // No more levels; Run has ended
        if (nextLevel == null)
        {
            EndRun();
        }
        else
        {
            LoadLevel(nextLevel, true, true);
        }
    }

    private void LoadLevel(GameSceneSO level, bool transitionOut, bool transitionIn)
    {
        // Disable input while loading
        askInputStateChange.RaiseEvent(InputState.Disabled);

        askLoadGameplayLevel.RaiseEvent(level, transitionOut, transitionIn, () =>
        {
            askSaveProfile.RaiseEvent(activeSaveContainer.profileSaveData);
        });
    }

    // Run ended, Load run finish scene
    private void EndRun()
    {
        askLoadGameplayLevel.RaiseEvent(gameplayKeyScenes.RunFinishMenuScene, true, true);
    }
    
    private void EnterRunLobbyFromMenu()
    {
        SetupWorldGeneration();
        LoadEntryLevel(false);
    }

    private void EnterRunLobby()
    {
        SetupWorldGeneration();
        LoadEntryLevel(true);
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
        
        activeSaveContainer.profileSaveData.statsData.playTime += TimeSpan.FromSeconds(Time.deltaTime);
    }

    private void ReturnToMainMenu()
    {
        askSaveProfile.RaiseEvent(activeSaveContainer.profileSaveData);
        askInputStateChange.RaiseEvent(InputState.Disabled);
        askGameStateChange.RaiseEvent(GameState.MainMenu);
    }
}
