using System;
using System.Collections;
using System.Collections.Generic;
using MushiCore.EditorAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// Entry point for the game
/// Loads the persistent manager and requests to load the initial game state (Should be main menu)
/// </summary>
public class GameInitializer : DescriptionMonoBehavior
{
    [ColorHeader("Invoking - Ask Game State Change Channel", ColorHeaderColor.InvokingChannels)]
    [SerializeField] private AssetReference askGameStateChange;
        
    [ColorHeader("Initialization Config", ColorHeaderColor.Config)]
    [SerializeField] private GameState entryGameState;
    
    [ColorHeader("Dependencies", ColorHeaderColor.Dependencies)]
    [SerializeField] private PersistentManagerSceneSO persistentManagerScene;

    private void Start()
    {
        // Load persistent managers scene
        var loadHandle = Addressables.LoadSceneAsync(persistentManagerScene.sceneReference.RuntimeKey, LoadSceneMode.Additive, true);
        loadHandle.Completed += OnPersistentManagersLoaded;
    }
    
    private void OnPersistentManagersLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        // Manually load game state change channel
        var channelLoadHandle = askGameStateChange.LoadAssetAsync<GameStateEventChannelSO>();
        channelLoadHandle.Completed += OnGameStateChannelLoaded;
    }

    private void OnGameStateChannelLoaded(AsyncOperationHandle<GameStateEventChannelSO> obj)
    {
        // Request to change into the default game state (Main Manager)
        var stateChangeChannel = obj.Result;
        stateChangeChannel.RaiseEvent(entryGameState);
        // Unload initialization scene
        SceneManager.UnloadSceneAsync(0);
    }
    
}
