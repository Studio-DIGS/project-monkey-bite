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
    [SerializeField] private SceneUnloadEventChannelSO askUnloadManagerScene;

    [ColorHeader("Invoking - On Manager Scene Ready Channels", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private VoidEventChannelSO onManagerSceneLoaded;

    [ColorHeader("Listening - Content Scene Ask Channels", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private SceneLoadEventChannelSO askLoadContentScene;
    [SerializeField] private SceneUnloadEventChannelSO askUnloadContentScene;

    [ColorHeader("Invoking - On Content Scene Ready Channels", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private VoidEventChannelSO onContentSceneLoaded;
    
    [ColorHeader("Writing - Scene State", ColorHeaderColor.WritingState)] 
    [SerializeField] private CurrentSceneStateSO currentSceneState;

    [ColorHeader("Dependencies", ColorHeaderColor.Dependencies)] 
    [SerializeField] private TransitionEffectManager transitionEffectManager;
    
    // Manager Scenes
    private AsyncOperationHandle<SceneInstance> currentManagerSceneHandle;
    private GameSceneSO currentlyLoadedManagerScene;
    
    // Content Scenes
    private AsyncOperationHandle<SceneInstance> currentContentSceneHandle;
    private GameSceneSO currentlyLoadedContentScene;
    
    // State
    private int sceneOperationCount;

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

    private void SceneTransitionEntryCheck()
    {
        if (sceneOperationCount > 0)
        {
            Debug.LogError("Attempted to start a scene operation while one was currently in progress");
        }
    }

    private void LoadManagerScene(GameSceneSO scene, bool transitionOut, bool transitionIn, Action loadScreenActions)
    {
        SceneTransitionEntryCheck();
        StartCoroutine(CoroutLoadManagerScene(scene, transitionOut, transitionIn, loadScreenActions));
    }

    private IEnumerator CoroutLoadManagerScene(GameSceneSO scene, bool transitionOut, bool transitionIn, Action loadScreenActions)
    {
        // Start async load manager scene
        var loadNewManagerSceneHandle = scene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, false, 0);

        // Transition out animation
        if (transitionOut)
        {
            yield return transitionEffectManager.TransitionOut();
        }
        
        // Run any load screen actions
        loadScreenActions?.Invoke();

        yield return loadNewManagerSceneHandle;

        // Done; New Manager scene has finished loading
        if (loadNewManagerSceneHandle.IsValid())
        {
            var newManagerScene = loadNewManagerSceneHandle.Result;
            
            // Start async unload any existing manager scenes
            var unloadRoutine = StartCoroutine(CoroutUnloadManagerScene(false, false, null));
            var activateOperation = newManagerScene.ActivateAsync();
            
            // Wait for both scene operations to complete
            yield return unloadRoutine;
            yield return activateOperation;
            
            SceneManager.SetActiveScene(newManagerScene.Scene);
            
            // Update handles and states
            currentlyLoadedManagerScene = scene;
            currentSceneState.currentlyLoadedManagerScene = scene;
            currentManagerSceneHandle = loadNewManagerSceneHandle;
            onManagerSceneLoaded.RaiseEvent();
        }

        // Transition out animation (does not affect load timing)
        if (transitionIn)
        {
            yield return transitionEffectManager.TransitionIn();
        }
    }
    
    private void UnloadManagerScene(bool transitionOut, bool transitionIn, Action loadScreenActions)
    {
        SceneTransitionEntryCheck();
        StartCoroutine(CoroutUnloadManagerScene(transitionOut, transitionIn, loadScreenActions));
    }

    private IEnumerator CoroutUnloadManagerScene(bool transitionOut, bool transitionIn, Action loadScreenActions)
    {
        // Transition out animation
        if (transitionOut)
        {
            yield return transitionEffectManager.TransitionOut();
        }
        
        // Start async unload any content scene
        var contentUnloadOperation = StartCoroutine(CoroutUnloadContentScene(false, false, null));
        
        // Unload current manager scene
        if (currentlyLoadedManagerScene != null)
        {
            if (currentManagerSceneHandle.IsValid())
            {
                loadScreenActions?.Invoke();
                yield return Addressables.UnloadSceneAsync(currentManagerSceneHandle, true);
            }

            yield return contentUnloadOperation;
            currentSceneState.currentlyLoadedManagerScene = null;
            currentlyLoadedManagerScene = null;
        }
        else
        {
            yield return contentUnloadOperation;
        }
        
        // Transition out animation
        if (transitionIn)
        {
            yield return transitionEffectManager.TransitionOut();
        }

    }

    private void LoadContentScene(GameSceneSO scene, bool transitionOut, bool transitionIn, Action loadScreenActions)
    {
        SceneTransitionEntryCheck();
        StartCoroutine(CoroutLoadContentScene(scene, transitionOut, transitionIn, loadScreenActions));
    }

    private IEnumerator CoroutLoadContentScene(GameSceneSO scene, bool transitionOut, bool transitionIn, Action loadScreenActions)
    {
        Debug.Log("Load Started");
        // Start async load new content scene
        var loadNewContentSceneHandle = scene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, false, 0);
        
        // Transition out animation
        if (transitionOut)
        {
            yield return transitionEffectManager.TransitionOut();
        }

        // Run any load screen actions
        loadScreenActions?.Invoke();

        // Wait for both scene operations to complete
        yield return loadNewContentSceneHandle;
        Debug.Log("Load Done");

        if (loadNewContentSceneHandle.IsValid())
        {
            var newContentScene = loadNewContentSceneHandle.Result;
            Debug.Log("Activate started");
            var activateHandle = newContentScene.ActivateAsync();
            Debug.Log("Unload Started");
            // Start async unload current content scene
            var unloadOperation = StartCoroutine(CoroutUnloadContentScene(false, false, null));

            yield return activateHandle;
            Debug.Log("Activate done");
            yield return unloadOperation;
            Debug.Log("Unload done");
            
            SceneManager.SetActiveScene(newContentScene.Scene);
            
            currentlyLoadedContentScene = scene;
            currentSceneState.currentlyLoadedContentScene = scene;
            currentContentSceneHandle = loadNewContentSceneHandle;
            onContentSceneLoaded.RaiseEvent();
        }
        
        // Transition out animation (does not affect load timing)
        if (transitionIn)
        {
            yield return transitionEffectManager.TransitionIn();
        }
    }
    
    private void UnloadContentScene(bool transitionOut, bool transitionIn, Action loadScreenActions)
    {
        SceneTransitionEntryCheck();
        StartCoroutine(CoroutUnloadContentScene(transitionOut, transitionIn, loadScreenActions));
    }

    private IEnumerator CoroutUnloadContentScene(bool transitionOut, bool transitionIn, Action loadScreenActions)
    {
        // Transition out animation
        if (transitionOut)
        {
            yield return transitionEffectManager.TransitionOut();
        }
        
        if (currentlyLoadedContentScene != null)
        {
            if (currentContentSceneHandle.IsValid())
            {
                loadScreenActions?.Invoke();
                yield return Addressables.UnloadSceneAsync(currentContentSceneHandle, true);
            }
            currentSceneState.currentlyLoadedContentScene = null;
            currentlyLoadedContentScene = null;
        }
        
        // Transition out animation
        if (transitionIn)
        {
            yield return transitionEffectManager.TransitionIn();
        }

    }
}
