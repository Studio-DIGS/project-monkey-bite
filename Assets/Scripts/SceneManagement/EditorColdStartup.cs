using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// This class deals with loading gameplay or menu scenes directly from the editor
/// </summary>
public class EditorColdStartup : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private GameSceneSO thisSceneData;
    [SerializeField] private GameSceneSO persistentManagerSceneData;
    [SerializeField] private bool isContentScene;
    
    [Header("Invoking - Cold Startup Channel")]
    [SerializeField] private ColdStartupEventChannelSO notifyColdStartupChannel;
    [HideInInspector, SerializeField] private VoidEventChannelSO managerSceneReadyChannel;
    
    [Header("Isolated Startup Dependencies")]
    [HideInInspector, SerializeField] private VoidEventChannelSO contentSceneReadyChannel;
    [HideInInspector, SerializeField] private RequestInputStateChangeEventChannelSO setInputStateChannel;
    [HideInInspector, SerializeField] private InputState startupInputState;
    
    private bool isColdStart = false;
    private void Awake()
    {
        if (!SceneManager.GetSceneByName(persistentManagerSceneData.sceneReference.editorAsset.name).isLoaded)
        {
            isColdStart = true;
        }
    }

    private void Start()
    {
        if (isColdStart)
        {
            Debug.Log("Cold Startup, Loading Persistent Managers");
            persistentManagerSceneData.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += OnPersistentManagersLoaded;
        }
    }

    private void OnPersistentManagersLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        if (thisSceneData != null)
        {
            ColdStartup();
        }
        else
        {
            IsolatedColdStartup();
        }
    }

    private void ColdStartup()
    {
        notifyColdStartupChannel.RaiseEvent(thisSceneData, isContentScene);
        if (!isContentScene)
        {
            managerSceneReadyChannel.RaiseEvent();
        }
    }

    private void IsolatedColdStartup()
    {
        // Isolated cold content 
        if (isContentScene)
        {
            contentSceneReadyChannel.RaiseEvent();
            setInputStateChannel.RaiseEvent(startupInputState);
        }
        // Isolated cold manager
        else
        {
            managerSceneReadyChannel.RaiseEvent();
        }
    }

#endif
}