using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private SceneLoadEventChannelSO loadLevelChannel;
    [SerializeField] private SceneUnloadAllEventChannelSO unloadGameplayScenesChannel;
    [SerializeField] private GameSceneSO gameplayEntryScene;
    [SerializeField] private RequestGameStateChangeEventChannelSO requestGameStateChange;
    [SerializeField] private RequestInputStateChangeEventChannelSO requestInputStateChange;
    [SerializeField] private CurrentSceneStateSO currentSceneState;
    private void Start()
    {
        if(currentSceneState.currentlyLoadedContentScene == null)
            loadLevelChannel.RaiseEvent(gameplayEntryScene);
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
