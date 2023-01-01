using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayManager : DescriptionMonoBehavior
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

    [ColorHeader("Listening - On Level Completed Channel", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private VoidEventChannelSO onLevelCompleted;
    
    [ColorHeader("Listening - Start new Run Ask Channel", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private VoidEventChannelSO askStartNewRun;
    
    [ColorHeader("Listening - Return To Main Menu Ask Channel", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private VoidEventChannelSO askReturnToMainMenu;
    
    [ColorHeader("Invoking - Ask Save Data Channel", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private SaveProfileDataEventChannelSO askSaveProfile;

    [ColorHeader("Dependencies", ColorHeaderColor.Dependencies)] 
    [SerializeField] private RunProgressionDirector runProgressionDirector;
    [SerializeField] private ProfileSaveDataSO activeSaveContainer;

    // Debug exit
    private bool exitToMenu;

    private void OnEnable()
    {
        onGameplayManagerSceneLoaded.OnRaised += SetupGameplayManager;
        onLevelSceneLoaded.OnRaised += OnLevelSceneLoaded;
        onLevelCompleted.OnRaised += LoadNextProgressionScene;
        askReturnToMainMenu.OnRaised += ReturnToMainMenu;
        askStartNewRun.OnRaised += StartNewRun;
    }

    private void OnDisable()
    {
        onGameplayManagerSceneLoaded.OnRaised -= SetupGameplayManager;
        onLevelSceneLoaded.OnRaised -= OnLevelSceneLoaded;
        onLevelCompleted.OnRaised -= LoadNextProgressionScene;
        askReturnToMainMenu.OnRaised -= ReturnToMainMenu;
        askStartNewRun.OnRaised -= StartNewRun;
    }

    private void SetupGameplayManager()
    {
        // Input disabled by default
        askInputStateChange.RaiseEvent(InputState.Disabled);
        
        runProgressionDirector.LoadstartRunProgressionDirector(activeSaveContainer.saveProfileData.runData);
        LoadNextProgressionScene();
    }

    private void OnLevelSceneLoaded()
    {
        // TODO: Any extra work done before the player is spawned goes here
        
        //  Raise the event indicating the level is ready, the level scene handles the rest
        onLevelSceneReady.RaiseEvent();
    }

    private void LoadNextProgressionScene()
    {
        RunProgressionDirector.RunProgressionData progressionData = runProgressionDirector.GetNextProgressionData();
        
        if (progressionData.startNewRun)
        {
            StartNewRun();
        }
        else
        {
            LoadLevel(progressionData.scene, progressionData.transitionOut, progressionData.transitionIn);
        }
    }
    
    private void StartNewRun()
    {
        runProgressionDirector.RestartRunProgressionDirector(activeSaveContainer.saveProfileData.runData);
        LoadNextProgressionScene();
    }
    
    private void LoadLevel(GameSceneSO level, bool transitionOut, bool transitionIn)
    {
        // Disable input while loading
        askInputStateChange.RaiseEvent(InputState.Disabled);

        // Save whenever loading a level
        askLoadGameplayLevel.RaiseEvent(level, transitionOut, transitionIn, () =>
        {
            askSaveProfile.RaiseEvent(activeSaveContainer.saveProfileData);
        });
    }


    private void ReturnToMainMenu()
    {
        // Save first
        askSaveProfile.RaiseEvent(activeSaveContainer.saveProfileData);
        
        // Disable input and ask to change to main menu
        askInputStateChange.RaiseEvent(InputState.Disabled);
        askGameStateChange.RaiseEvent(GameState.MainMenu);
    }

    private void Update()
    {
        // Simple debugging for exiting
        if (Input.GetKey(KeyCode.Escape) && !exitToMenu)
        {
            exitToMenu = true;
            askReturnToMainMenu.RaiseEvent();
        }

        activeSaveContainer.saveProfileData.statsData.playTime += Time.deltaTime;
    }
}
