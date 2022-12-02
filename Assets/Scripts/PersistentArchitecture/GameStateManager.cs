using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Initializing,
    MainMenu,
    Gameplay
}

public class GameStateManager : MonoBehaviour
{
    [Header("Listening - Request Game State Change Channels")] 
    [SerializeField] private RequestGameStateChangeEventChannelSO requestGameStateChange;

#if UNITY_EDITOR
    [Header("Listening - Cold Startup Channel")]
    [SerializeField] private ColdStartupEventChannelSO coldStartupChannel;
#endif
    
    [Header("Invoking - Load Manager Scenes Channel")] 
    [SerializeField] private SceneLoadEventChannelSO loadManagerSceneChannel;

    [Header("State Entry Scenes")] 
    [SerializeField] private GameSceneSO mainMenuScene;
    [SerializeField] private GameSceneSO gameplayScene;

    private GameState currentGameState = GameState.Initializing;

    private void OnEnable()
    {
        requestGameStateChange.OnRequestGameStateChange += TryChangeGameState;
#if UNITY_EDITOR
        coldStartupChannel.OnColdStartup += ColdStartup;
#endif
    }

    private void OnDisable()
    {
        requestGameStateChange.OnRequestGameStateChange -= TryChangeGameState;
#if UNITY_EDITOR
        coldStartupChannel.OnColdStartup -= ColdStartup;
#endif
    }
    
    private void TryChangeGameState(GameState newState)
    {
        if (currentGameState == newState)
        {
            Debug.LogWarning($"Tried to change to already active game state {newState}");
        }

        // Exit behavior for current state
        switch (currentGameState)
        {
            case GameState.MainMenu:
                ExitMainMenuState();
                break;
            case GameState.Gameplay:
                ExitGameplayState();
                break;
        }
        
        // Enter behavior for new state
        switch (newState)
        {
            case GameState.MainMenu:
                EnterMainMenuState();
                break;
            case GameState.Gameplay:
                EnterGameplayState();
                break;
        }
        
        currentGameState = newState;
    }

    private void EnterGameplayState()
    {
        loadManagerSceneChannel.RaiseEvent(gameplayScene, true, true);
    }

    private void ExitGameplayState()
    {
        
    }

    private void EnterMainMenuState()
    {
        loadManagerSceneChannel.RaiseEvent(mainMenuScene, true, true);
    }

    private void ExitMainMenuState()
    {
        
    }
    
#if UNITY_EDITOR
    /// <summary>
    /// Enter the correct game state matching the cold startup scene
    /// </summary>
    /// <param name="startupScene"></param>
    /// <param name="isContentScene"></param>
    private void ColdStartup(GameSceneSO startupScene, bool isContentScene)
    {
        // If its a content scene, then enter the proper game state
        if (isContentScene)
        {
            switch (startupScene.sceneType)
            {
                case GameSceneSO.GameSceneType.GameplayLevel:
                    TryChangeGameState(GameState.Gameplay);
                    break;
            }
        }
        // If its a manager scene, then just set the game state
        else
        {
            switch (startupScene.sceneType)
            {
                case GameSceneSO.GameSceneType.Menu:
                    currentGameState = GameState.MainMenu;
                    break;
            }
        }
    }
#endif
}
