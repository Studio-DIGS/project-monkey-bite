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
public class PersistentSceneManager : DescriptionMonoBehavior
{
    [ColorHeader("Listening", ColorHeaderColor.ListeningChannels)]
    [ColorHeader("Manager Scene Ask Channels")] 
    [SerializeField] private SceneLoadEventChannelSO askLoadManagerScene;
    [SerializeField] private SceneUnloadEventChannelSO askUnloadManagerScene;
    
    [ColorHeader("Content Scene Ask Channels")] 
    [SerializeField] private SceneLoadEventChannelSO askLoadContentScene;
    [SerializeField] private SceneUnloadEventChannelSO askUnloadContentScene;

    [ColorHeader("Invoking", ColorHeaderColor.InvokingChannels)]
    [ColorHeader("On Manager Scene Ready")]
    [SerializeField] private VoidEventChannelSO onManagerSceneLoaded;
    
    [ColorHeader("On Content Scene Ready")]
    [SerializeField] private VoidEventChannelSO onContentSceneLoaded;
    
    [ColorHeader("Writing - Scene State", ColorHeaderColor.WritingState)] 
    [SerializeField] private CurrentSceneStateSO sceneStateBoard;

    [ColorHeader("Dependencies", ColorHeaderColor.Dependencies)] 
    [SerializeField] private TransitionEffectManager transitionEffectManager;
    
    public struct LoadedSceneData
    {
        public AsyncOperationHandle<SceneInstance> sceneInstanceHandle;
        public GameSceneSO sceneData;
    }
    
    private const int managerLayerIndex = 0;
    private const int contentLayerIndex = 1;
    private const int layerCount = 2;

    private LoadedSceneData[] LoadedScenes = new LoadedSceneData[layerCount];
    
    // State
    private int sceneOperationCount;

    private Coroutine currentOperation;
    private Coroutine CurrentOperation
    {
        get => currentOperation;
        set
        {
            currentOperation = value;
            sceneStateBoard.canStartNewSceneOperation = (value == null);
        }
    }

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

    private void LoadManagerScene(GameSceneSO scene, bool transitionOut, bool transitionIn, Action loadScreenActions)
    {
        bool sceneAlreadyLoaded = LoadedScenes[managerLayerIndex].sceneData == scene;
        if (!SceneTransitionEntryCheck() || sceneAlreadyLoaded) return;
        CurrentOperation = StartCoroutine(CoroutLoadScene(
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
        CurrentOperation = StartCoroutine(CoroutUnloadScene(transitionOut, transitionIn, managerLayerIndex));
    }
    
    private void LoadContentScene(GameSceneSO scene, bool transitionOut, bool transitionIn, Action loadScreenActions)
    {
        bool sceneAlreadyLoaded = LoadedScenes[contentLayerIndex].sceneData == scene;
        if (!SceneTransitionEntryCheck() || sceneAlreadyLoaded) return;
        CurrentOperation = StartCoroutine(CoroutLoadScene(
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
        CurrentOperation = StartCoroutine(CoroutUnloadScene(transitionOut, transitionIn, contentLayerIndex));
    }
    
    private bool SceneTransitionEntryCheck()
    {
        if (CurrentOperation != null)
        {
            Debug.LogError("Attempted to start a scene operation while one was currently in progress");
            return false;
        }

        return true;
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

        // Done; New Scene has finished loading
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
            LoadedScenes[layerIndex].sceneData = scene;
            LoadedScenes[layerIndex].sceneInstanceHandle = loadNewManagerSceneHandle;
            
            // Raise events
            CurrentOperation = null;
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
        if (LoadedScenes[layerIndex].sceneData != null)
        {
            if (LoadedScenes[layerIndex].sceneInstanceHandle.IsValid())
            {
                yield return Addressables.UnloadSceneAsync(LoadedScenes[layerIndex].sceneInstanceHandle, true);
            }

            yield return recursiveUnloadOperation;
            LoadedScenes[layerIndex].sceneData = null;
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
