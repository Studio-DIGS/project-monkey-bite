using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [Header("Listening - Manager Scene Channels")] 
    [SerializeField] private SceneLoadEventChannelSO managerSceneLoadChannel;
    [SerializeField] private SceneUnloadAllEventChannelSO managerSceneUnloadChannel;
    
    [Header("Listening - Content Scene Channels")] 
    [SerializeField] private SceneLoadEventChannelSO contentSceneLoadChannel;
    [SerializeField] private SceneUnloadAllEventChannelSO contentSceneUnloadChannel;
    
#if UNITY_EDITOR
    [Header("Listening - Cold Startup Channel")]
    [SerializeField] private ColdStartupEventChannelSO coldStartupChannelSo;
#endif

    [Header("Writing - Scene State SO")] 
    [SerializeField] private CurrentSceneStateSO currentSceneState;
    
    // Manager Scenes
    private AsyncOperationHandle<SceneInstance> currentLoadedManagerHandle;
    private GameSceneSO currentlyLoadedManagerScene;
    
    // Content Scenes
    private AsyncOperationHandle<SceneInstance> currentLoadedContentHandle;
    private GameSceneSO currentlyLoadedContentScene;
    
    // State
    private bool isInSceneTransition;

    private void OnEnable()
    {
        managerSceneLoadChannel.OnLoadingRequested += LoadManagerScene;
        managerSceneUnloadChannel.OnUnloadingRequested += UnloadManagerScene;
        
        contentSceneLoadChannel.OnLoadingRequested += LoadContentScene;
        contentSceneUnloadChannel.OnUnloadingRequested += UnloadContentScene;
#if UNITY_EDITOR
        coldStartupChannelSo.OnColdStartup += ColdStartup;
#endif
    }
    
    private void OnDisable()
    {
        managerSceneLoadChannel.OnLoadingRequested -= LoadManagerScene;
        managerSceneUnloadChannel.OnUnloadingRequested -= UnloadManagerScene;
        
        contentSceneLoadChannel.OnLoadingRequested -= LoadContentScene;
        contentSceneUnloadChannel.OnUnloadingRequested -= UnloadContentScene;
        
#if UNITY_EDITOR
        coldStartupChannelSo.OnColdStartup -= ColdStartup;
#endif
    }

#if UNITY_EDITOR
    private void ColdStartup(GameSceneSO scene, bool isContentScene)
    {
        if (isContentScene)
        {
            currentlyLoadedContentScene = scene;
            currentSceneState.currentlyLoadedContentScene = scene;
        }
        else
        {
            currentlyLoadedManagerScene = scene;
            currentSceneState.currentlyLoadedManagerScene = scene;
        }
    }
#endif
    
    private void LoadManagerScene(GameSceneSO scene, bool transition, bool fade)
    {
        UnloadManagerScene();
        currentLoadedManagerHandle = scene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
        currentLoadedManagerHandle.Completed += (handle) =>
        {
            currentlyLoadedManagerScene = scene;
            currentSceneState.currentlyLoadedManagerScene = scene;
        };
    }
    
    private void UnloadManagerScene()
    {
        UnloadContentScene();
        if (currentlyLoadedManagerScene != null)
        {
            if (currentLoadedManagerHandle.IsValid())
            {
                var unloadManagerHandle = Addressables.UnloadSceneAsync(currentLoadedManagerHandle, true);
            }
#if UNITY_EDITOR
            else
            {
                // Unload cold startup scene through SceneManager
                SceneManager.UnloadSceneAsync(currentlyLoadedManagerScene.sceneReference.editorAsset.name);
            }
#endif
            currentSceneState.currentlyLoadedManagerScene = null;
            currentlyLoadedManagerScene = null;
        }
    }

    private void LoadContentScene(GameSceneSO scene, bool transition, bool fade)
    {
        UnloadContentScene();
        currentLoadedContentHandle = scene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
        currentLoadedContentHandle.Completed += (handle) =>
        {
            SceneManager.SetActiveScene(handle.Result.Scene);
            currentlyLoadedContentScene = scene;
            currentSceneState.currentlyLoadedContentScene = scene;
        };
    }
    
    private void UnloadContentScene()
    {
        if (currentlyLoadedContentScene != null)
        {
            if (currentLoadedContentHandle.IsValid())
            {
                var unloadContentHandle = Addressables.UnloadSceneAsync(currentLoadedContentHandle, true);
            }
#if UNITY_EDITOR
            else
            {
                // Unload cold startup scene through SceneManager
                SceneManager.UnloadSceneAsync(currentlyLoadedContentScene.sceneReference.editorAsset.name);
            }
#endif
            currentSceneState.currentlyLoadedContentScene = null;
            currentlyLoadedContentScene = null;
        }
    }
}
