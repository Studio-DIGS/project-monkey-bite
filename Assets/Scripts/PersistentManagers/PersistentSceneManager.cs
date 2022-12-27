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

    struct LoadedSceneData
    {
        public AsyncOperationHandle<SceneInstance> sceneInstanceHandle;
        public GameSceneSO sceneData;
    }
    
    private const int managerLayerIndex = 0;
    private const int contentLayerIndex = 1;
    private const int layerCount = 2;

    private LoadedSceneData[] loadedScenes = new LoadedSceneData[layerCount];
    
    // State
    private int sceneOperationCount;
    private Coroutine currentOperation;

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

    private bool SceneTransitionEntryCheck()
    {
        if (currentOperation != null)
        {
            Debug.LogError("Attempted to start a scene operation while one was currently in progress");
            return false;
        }

        return true;
    }

    private void LoadManagerScene(GameSceneSO scene, bool transitionOut, bool transitionIn, Action loadScreenActions)
    {
        if (!SceneTransitionEntryCheck()) return;
        currentOperation = StartCoroutine(CoroutLoadScene(
            scene, 
            transitionOut, 
            transitionIn, 
            loadScreenActions, 
            managerLayerIndex,
            onManagerSceneLoaded
            ));
    }
        
    private void UnloadManagerScene(bool transitionOut, bool transitionIn)
    {
        if (!SceneTransitionEntryCheck()) return;
        currentOperation = StartCoroutine(CoroutUnloadScene(transitionOut, transitionIn, managerLayerIndex));
    }
    
    private void LoadContentScene(GameSceneSO scene, bool transitionOut, bool transitionIn, Action loadScreenActions)
    {
        if (!SceneTransitionEntryCheck()) return;
        currentOperation = StartCoroutine(CoroutLoadScene(
            scene, 
            transitionOut, 
            transitionIn, 
            loadScreenActions, 
            contentLayerIndex,
            onContentSceneLoaded
            ));
    }

    private void UnloadContentScene(bool transitionOut, bool transitionIn)
    {
        if (!SceneTransitionEntryCheck()) return;
        currentOperation = StartCoroutine(CoroutUnloadScene(transitionOut, transitionIn, contentLayerIndex));
    }

    private IEnumerator CoroutLoadScene(
        GameSceneSO scene, 
        bool transitionOut, 
        bool transitionIn, 
        Action loadScreenActions, 
        int layerIndex,
        VoidEventChannelSO onFinishedChannel)
    {
        // Start async load manager scene
        var loadNewManagerSceneHandle = scene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, false, 0);

        yield return TryTransitionOutEffect(transitionOut);
        
        // Run any load screen actions
        loadScreenActions?.Invoke();
        
        // Async unload the previous scene
        var unloadOperation = StartCoroutine(CoroutUnloadScene(false, false, layerIndex));

        // Wait for scene to load
        yield return loadNewManagerSceneHandle;

        // Done; New Manager scene has finished loading
        if (loadNewManagerSceneHandle.IsValid())
        {
            var newManagerScene = loadNewManagerSceneHandle.Result;
 
            // Async activate the newly loaded scene
            var activateOperation = newManagerScene.ActivateAsync();

            // Wait for both to complete
            yield return activateOperation;
            yield return unloadOperation;
            
            // Set newly loaded scene as the "active" scene
            SceneManager.SetActiveScene(newManagerScene.Scene);
            
            // Update state
            loadedScenes[layerIndex].sceneData = scene;
            currentSceneState.currentlyLoadedManagerScene = scene;
            loadedScenes[layerIndex].sceneInstanceHandle = loadNewManagerSceneHandle;
            
            // Raise events
            currentOperation = null;
            onFinishedChannel.RaiseEvent();
        }

        // Transition out animation (does not affect load timing)
        yield return TryTransitionInEffect(transitionIn);
    }

    private IEnumerator CoroutUnloadScene(bool transitionOut, bool transitionIn, int layerIndex)
    {
        yield return TryTransitionOutEffect(transitionOut);
        
        // Recursvely unload any scenes in a lower layer (Higher index)
        Coroutine recursiveUnloadOperation = null;
        if (layerIndex + 1 < layerCount)
        {
            recursiveUnloadOperation = StartCoroutine(CoroutUnloadScene(false, false, layerIndex + 1));
        }

        // Unload current manager scene
        if (loadedScenes[layerIndex].sceneData != null)
        {
            if (loadedScenes[layerIndex].sceneInstanceHandle.IsValid())
            {
                yield return Addressables.UnloadSceneAsync(loadedScenes[layerIndex].sceneInstanceHandle, true);
            }

            yield return recursiveUnloadOperation;
            currentSceneState.currentlyLoadedManagerScene = null;
            loadedScenes[layerIndex].sceneData = null;
        }
        else
        {
            yield return recursiveUnloadOperation;
        }

        yield return TryTransitionInEffect(transitionIn);

    }

    private IEnumerator TryTransitionInEffect(bool shouldTransitionIn)
    {
        if (shouldTransitionIn)
        {
            yield return transitionEffectManager.TransitionIn();
        }
    }
    
    private IEnumerator TryTransitionOutEffect(bool shouldTransitionOut)
    {
        if (shouldTransitionOut)
        {
            yield return transitionEffectManager.TransitionOut();
        }
    }
}
