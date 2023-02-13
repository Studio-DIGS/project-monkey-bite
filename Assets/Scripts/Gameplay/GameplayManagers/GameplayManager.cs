using System;
using System.Collections;
using System.Collections.Generic;
using MushiCore.EditorAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayManager : DescriptionMonoBehavior
{
    [ColorHeader("Listening", ColorHeaderColor.ListeningChannels)]
    [ColorHeader("On Scene Loaded Channels")]
    [SerializeField] private VoidEventChannelSO onGameplayManagerSceneLoaded;
    [SerializeField] private VoidEventChannelSO onLevelSceneLoaded;
    
    [ColorHeader("Ask Progress Level")] 
    [SerializeField] private VoidEventChannelSO askProgressLevel;
    
    [ColorHeader("Start New Run Ask")] 
    [SerializeField] private VoidEventChannelSO askStartNewRun;
    
    [ColorHeader("Return To Main Menu Ask")] 
    [SerializeField] private VoidEventChannelSO askReturnToMainMenu;

    [ColorHeader("Invoking", ColorHeaderColor.InvokingChannels, true)]
    [ColorHeader("On Level Ready")]
    [SerializeField] private VoidEventChannelSO onLevelSceneReady;
    
    [ColorHeader("Ask Input State Change")]
    [SerializeField] private InputStateEventChannelSO askInputStateChange;

    [ColorHeader("Ask Game State Change")]
    [SerializeField] private GameStateEventChannelSO askGameStateChange;
    
    [ColorHeader("Ask Scene Management")]
    [SerializeField] private SceneLoadEventChannelSO askLoadGameplayLevel;

    [ColorHeader("Ask Save Profile To File")] 
    [SerializeField] private SaveProfileDataEventChannelSO askSaveProfile;

    [ColorHeader("Dependencies", ColorHeaderColor.Dependencies, true)] 
    [SerializeField] private RunProgressionDirector runProgressionDirector;
    [SerializeField] private ProfileSaveDataSO activeSaveProfileBoard;
    [SerializeField] private CurrentSceneStateSO sceneStateBoard;

    // Debug exit
    private bool exitToMenu;

    private void OnEnable()
    {
        onGameplayManagerSceneLoaded.OnRaised += SetupGameplayManager;
        onLevelSceneLoaded.OnRaised += OnLevelSceneLoaded;
        askProgressLevel.OnRaised += LoadNextProgressionScene;
        askReturnToMainMenu.OnRaised += ReturnToMainMenu;
        askStartNewRun.OnRaised += StartNewRun;
    }

    private void OnDisable()
    {
        onGameplayManagerSceneLoaded.OnRaised -= SetupGameplayManager;
        onLevelSceneLoaded.OnRaised -= OnLevelSceneLoaded;
        askProgressLevel.OnRaised -= LoadNextProgressionScene;
        askReturnToMainMenu.OnRaised -= ReturnToMainMenu;
        askStartNewRun.OnRaised -= StartNewRun;
    }

    private void SetupGameplayManager()
    {
        // Input disabled by default
        askInputStateChange.RaiseEvent(InputState.Disabled);
        
        runProgressionDirector.LoadstartRunProgressionDirector(activeSaveProfileBoard.saveProfileData.runData);
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
        if (!sceneStateBoard.canStartNewSceneOperation) return;
        
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
        runProgressionDirector.RestartRunProgressionDirector(activeSaveProfileBoard.saveProfileData.runData);
        LoadNextProgressionScene();
    }
    
    private void LoadLevel(GameSceneSO level, bool transitionOut, bool transitionIn)
    {
        // Disable input while loading
        askInputStateChange.RaiseEvent(InputState.Disabled);

        // Save whenever loading a level
        askLoadGameplayLevel.RaiseEvent(level, transitionOut, transitionIn, () =>
        {
            askSaveProfile.RaiseEvent(activeSaveProfileBoard.saveProfileData);
        });
    }


    private void ReturnToMainMenu()
    {
        // Save first
        askSaveProfile.RaiseEvent(activeSaveProfileBoard.saveProfileData);
        
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

        activeSaveProfileBoard.saveProfileData.statsData.playTime += Time.deltaTime;
    }
}
