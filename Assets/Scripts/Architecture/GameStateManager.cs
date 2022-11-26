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
    
    [Header("Invoking - Request Input State Change Channels")]
    [SerializeField] private RequestInputStateChangeEventChannelSO requestInputStateChange;

    [Header("Invoking - Scene Load Channels")] 
    [SerializeField] private SceneLoadEventChannelSO loadManagerSceneChannel;

    [Header("State Entry Scenes")] 
    [SerializeField] private GameSceneSO mainMenuScene;
    [SerializeField] private GameSceneSO gameplayManagersScene;

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
    
#if UNITY_EDITOR
    private void ColdStartup(GameSceneSO startupScene, bool isContentScene)
    {
        switch (startupScene.sceneType)
        {
            case GameSceneSO.GameSceneType.Menu:
                TryChangeGameState(GameState.MainMenu);
                break;
            case GameSceneSO.GameSceneType.GameplayLevel:
                TryChangeGameState(GameState.Gameplay);
                break;
        }
    }
#endif
    private void TryChangeGameState(GameState newState)
    {
        if (currentGameState == newState)
        {
            Debug.LogWarning($"Tried to change to current game state {newState}");
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
        requestInputStateChange.RaiseEvent(InputState.UI);
        loadManagerSceneChannel.RaiseEvent(gameplayManagersScene, true, true);
    }

    private void ExitGameplayState()
    {
        
    }

    private void EnterMainMenuState()
    {
        requestInputStateChange.RaiseEvent(InputState.UI);
        loadManagerSceneChannel.RaiseEvent(mainMenuScene, true, true);
    }

    private void ExitMainMenuState()
    {
        
    }
}
