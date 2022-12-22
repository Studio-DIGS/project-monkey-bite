using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// This class deals with loading gameplay or menu scenes directly from the editor 
/// </summary>
public class EditorColdStartup : MonoBehaviour
{
#if UNITY_EDITOR
    [ColorHeader("Scene Dependencies", ColorHeaderColor.Dependencies)]
    [SerializeField] private GameSceneSO thisSceneData;
    [SerializeField] private GameSceneSO persistentManagerSceneData;
    
    [ColorHeader("Writing - Cold Startup Data", ColorHeaderColor.WritingState)]
    [SerializeField] private ColdStartupDataSO coldStartupData;
    
    [ColorHeader("Isolated Startup Config", ColorHeaderColor.Config)]
    [HideInInspector, SerializeField] private InputState startupInputState;
    
    [ColorHeader("Invoking - Isolated Startup Setup Channels", ColorHeaderColor.TriggeringEvents)]
    [HideInInspector, SerializeField] private InputStateEventChannelSO askInputStateChange;
    [HideInInspector, SerializeField] private VoidEventChannelSO[] isolatedManualRaiseChannels;

    private bool isColdStart = false;
    
    private void Awake()
    {
        // Cold start if persistent manager is not loaded
        if (!SceneManager.GetSceneByName(persistentManagerSceneData.sceneReference.editorAsset.name).isLoaded)
        {
            isColdStart = true;
        }
    }

    private void Start()
    {
        if (isColdStart)
        {
            // First load Persistent Managers scene
            coldStartupData.isColdStartup = true;
            persistentManagerSceneData.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, false).Completed += OnPersistentManagersLoaded;
        }
    }
    
    private void OnPersistentManagersLoaded(AsyncOperationHandle<SceneInstance> persistentManagerScene)
    {
        if (thisSceneData != null)
        {
            Debug.LogWarning("Cold Startup Initialized");
            ColdStartup(persistentManagerScene);
        }
        else
        {
            Debug.LogWarning("No Game Scene Specified, Isolated Cold Startup Initialized");
            IsolatedColdStartup(persistentManagerScene);
        }
    }

    /// <summary>
    /// Normal Cold Startup - update cold startup data and activate persistent managers.
    /// </summary>
    /// <param name="scene"></param>
    private void ColdStartup(AsyncOperationHandle<SceneInstance> scene)
    {
        coldStartupData.startupScene = thisSceneData;
        var currentScene = SceneManager.GetActiveScene();
        scene.Result.ActivateAsync().completed += (a) =>
        {
            SceneManager.UnloadSceneAsync(currentScene);
        };
    }

    /// <summary>
    /// Isolated Cold Startup - No assigned "this" Game Scene.
    /// No game state level managers in this scenario, so various event channels
    /// need to be manually triggered
    /// </summary>
    /// <param name="scene"></param>
    private void IsolatedColdStartup(AsyncOperationHandle<SceneInstance> scene)
    {
        scene.Result.ActivateAsync().completed += (a) =>
        {
            askInputStateChange.RaiseEvent(startupInputState);
            
            // Manually trigger necessary events
            foreach (var manualTrigger in isolatedManualRaiseChannels)
            {
                manualTrigger.RaiseEvent();
            }
        };
    }

    public bool IsIsolated()
    {
        return thisSceneData == null;
    }

#endif
}