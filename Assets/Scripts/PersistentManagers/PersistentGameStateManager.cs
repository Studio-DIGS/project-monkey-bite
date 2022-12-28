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

/// <summary>
/// Simple state machine for managing the current game state
/// </summary>
public class PersistentGameStateManager : MonoBehaviour
{
    [ColorHeader("Listening - Game State Change Ask Channel", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private GameStateEventChannelSO askGameStateChange;

    [ColorHeader("Invoking - Ask Manager Scene Load Channel", ColorHeaderColor.TriggeringEvents)] 
    [SerializeField] private SceneLoadEventChannelSO askLoadManagerScene;

    [ColorHeader("Game State Manager Scenes", ColorHeaderColor.Dependencies)] 
    [SerializeField] private GameSceneSO mainMenuManagers;
    [SerializeField] private GameSceneSO gameplayManagers;
    
#if UNITY_EDITOR
    [ColorHeader("Reading - Cold Startup State", ColorHeaderColor.ReadingState)]
    [SerializeField] private ColdStartupDataSO coldStartupState;
#endif

    private GameState currentGameState = GameState.Initializing;

    private void OnEnable()
    {
        askGameStateChange.OnRaised += TryChangeGameState;
    }

    private void Start()
    {
#if UNITY_EDITOR
        if (coldStartupState.isColdStartup)
        {
            ColdStartup(coldStartupState);
        }
#endif
    }

    private void OnDisable()
    {
        askGameStateChange.OnRaised -= TryChangeGameState;
    }
    
    private void TryChangeGameState(GameState newState)
    {
        if (currentGameState == newState)
        {
            Debug.LogWarning($"Tried to change to already active game state {newState}");
            return;
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
        bool transitionOut = true;
        
        // Skip transition for cold startup
#if UNITY_EDITOR
        if (coldStartupState.isColdStartup)
        {
            transitionOut = false;
        }
#endif
        
        askLoadManagerScene.RaiseEvent(gameplayManagers, transitionOut, false);
    }

    private void ExitGameplayState()
    {
        
    }

    private void EnterMainMenuState()
    {
        askLoadManagerScene.RaiseEvent(mainMenuManagers, true, true);
    }

    private void ExitMainMenuState()
    {
        
    }
    
#if UNITY_EDITOR
    /// <summary>
    /// Performs game state cold startup from cold startup data
    /// Hard coded conditionals to translate scene type to the appropriate game state
    /// </summary>
    /// <param name="data"></param>
    private void ColdStartup(ColdStartupDataSO data)
    {
        // If its a content scene, then enter the proper game state
        var sceneAsset = data.startupScene;

        // Isolated cold startup, do nothing
        if (sceneAsset == null)
        {
            return;
        }
        
        data.SetColdStartupSaveProfileActive();
        
        // Translate scene type into appropriate game state
        switch (sceneAsset.sceneType)
        {
            case GameSceneSO.GameSceneType.Initialization:
                Debug.LogError("Attempted to Cold Load Initialization (Invalid Cold Load)");
                break;
            case GameSceneSO.GameSceneType.PersistentManagers:
                Debug.LogError("Attempted to Cold Load Persistent Managers (Invalid Cold Load)");
                break;
            case GameSceneSO.GameSceneType.GameplayLevel:
                TryChangeGameState(GameState.Gameplay);
                break;
            case GameSceneSO.GameSceneType.GameplayManager:
                TryChangeGameState(GameState.Gameplay);
                break;
            case GameSceneSO.GameSceneType.MainMenuContent:
                TryChangeGameState(GameState.MainMenu);
                break;
            case GameSceneSO.GameSceneType.MainMenuManager:
                TryChangeGameState(GameState.MainMenu);
                break;
        }
    }
#endif
}
