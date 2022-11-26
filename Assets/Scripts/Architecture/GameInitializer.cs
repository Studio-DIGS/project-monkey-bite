using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private PersistentManagerSceneSO persistentManagers;
    [SerializeField] private GameState entryState;
    
    // Make sure this channel is loaded
    [SerializeField] private AssetReference gameStateChangeChannel;
    private void Start()
    {
        var loadHandle = Addressables.LoadSceneAsync(persistentManagers.sceneReference.RuntimeKey, LoadSceneMode.Additive, true);
        loadHandle.Completed += LoadSceneLoadChannel;
    }
    
    private void LoadSceneLoadChannel(AsyncOperationHandle<SceneInstance> obj)
    {
        var channelLoadHandle = gameStateChangeChannel.LoadAssetAsync<RequestGameStateChangeEventChannelSO>();
        channelLoadHandle.Completed += LoadEntryScene;
    }

    private void LoadEntryScene(AsyncOperationHandle<RequestGameStateChangeEventChannelSO> obj)
    {
        var stateChangeChannel = obj.Result;
        stateChangeChannel.RaiseEvent(entryState);
        SceneManager.UnloadSceneAsync(0);
    }
    
}
