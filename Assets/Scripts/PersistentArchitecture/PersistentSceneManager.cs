using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles loading and unloading scenes one two levels - manager and content level.
/// Does not handle any decision logic of loading scenes
/// that should be handled on the individual game state manager level
/// </summary>
public class PersistentSceneManager : MonoBehaviour
{
    [ColorHeader("Listening - Manager Scene Ask Channels", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private SceneLoadEventChannelSO askLoadManagerScene;
    [SerializeField] private SceneUnloadAllEventChannelSO askUnloadManagerScene;

    [ColorHeader("Invoking - On Manager Scene Ready Channels", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private VoidEventChannelSO onManagerSceneLoaded;

    [ColorHeader("Listening - Content Scene Ask Channels", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private SceneLoadEventChannelSO askLoadContentScene;
    [SerializeField] private SceneUnloadAllEventChannelSO askUnloadContentScene;

    [ColorHeader("Invoking - On Content Scene Ready Channels", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private VoidEventChannelSO onContentSceneLoaded;
    
    [ColorHeader("Writing - Scene State", ColorHeaderColor.WritingState)] 
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
        askLoadManagerScene.OnRaised += LoadManagerScene;
        askUnloadManagerScene.OnRaised += UnloadManagerScene;
        
        askLoadContentScene.OnRaised += LoadContentScene;
        askUnloadContentScene.OnRaised += UnloadContentScene;
    }
    
    private void OnDisable()
    {
        askLoadManagerScene.OnRaised -= LoadManagerScene;
        askUnloadManagerScene.OnRaised -= UnloadManagerScene;
        
        askLoadContentScene.OnRaised -= LoadContentScene;
        askUnloadContentScene.OnRaised -= UnloadContentScene;
    }

    private void LoadManagerScene(GameSceneSO scene, bool transition, bool fade)
    {
        UnloadManagerScene();
        currentLoadedManagerHandle = scene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
        currentLoadedManagerHandle.Completed += (handle) =>
        {
            SceneManager.SetActiveScene(handle.Result.Scene);
            currentlyLoadedManagerScene = scene;
            currentSceneState.currentlyLoadedManagerScene = scene;
            onManagerSceneLoaded.RaiseEvent();
        };
    }
    
    private void UnloadManagerScene()
    {
        // Unloading manager scene unloads any content scenes by default
        UnloadContentScene();
        if (currentlyLoadedManagerScene != null)
        {
            if (currentLoadedManagerHandle.IsValid())
            {
                Addressables.UnloadSceneAsync(currentLoadedManagerHandle, true);
            }
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
            onContentSceneLoaded.RaiseEvent();
        };
    }
    
    private void UnloadContentScene()
    {
        if (currentlyLoadedContentScene != null)
        {
            if (currentLoadedContentHandle.IsValid())
            {
                Addressables.UnloadSceneAsync(currentLoadedContentHandle, true);
            }
            currentSceneState.currentlyLoadedContentScene = null;
            currentlyLoadedContentScene = null;
        }
    }
}
