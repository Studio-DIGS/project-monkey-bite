using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [Header("Listening")]
    [SerializeField] private VoidEventChannelSO contentSceneLoaded;
    
    [Header("Invoking")]
    [SerializeField] private VoidEventChannelSO gameplaySceneReady;
    [SerializeField] private RequestGameStateChangeEventChannelSO requestGameStateChange;
    [SerializeField] private RequestInputStateChangeEventChannelSO requestInputStateChange;
    [SerializeField] private SceneLoadEventChannelSO loadLevelChannel;
    [SerializeField] private SceneUnloadAllEventChannelSO unloadGameplayScenesChannel;
    
    [Header("Dependencies")]
    [SerializeField] private GameSceneSO gameplayEntryScene;
    [SerializeField] private CurrentSceneStateSO currentSceneState;


    private void Start()
    {
        void GameplaySceneReady()
        {
            gameplaySceneReady.RaiseEvent();
            contentSceneLoaded.OnEventRaised -= GameplaySceneReady;
        }
        if (currentSceneState.currentlyLoadedContentScene == null)
        {
            contentSceneLoaded.OnEventRaised += GameplaySceneReady;
            loadLevelChannel.RaiseEvent(gameplayEntryScene);
        }
        else
        {
            gameplaySceneReady.RaiseEvent();
        }
        requestInputStateChange.RaiseEvent(InputState.Gameplay);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMainMenu();
        }
    }

    private void ReturnToMainMenu()
    {
        unloadGameplayScenesChannel.RaiseEvent();
        requestGameStateChange.RaiseEvent(GameState.MainMenu);
    }
}
