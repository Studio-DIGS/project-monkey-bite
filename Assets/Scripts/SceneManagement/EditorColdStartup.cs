using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class EditorColdStartup : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private GameSceneSO thisSceneData;
    [SerializeField] private GameSceneSO persistentManagerSceneData;
    [SerializeField] private bool isContentScene;
    
    [Header("Invoking - Cold Startup Channel")]
    [SerializeField] private ColdStartupEventChannelSO notifyColdStartupChannel;
    
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
            notifyColdStartupChannel.RaiseEvent(thisSceneData, isContentScene);
        }
        else
        {
            // Cold Startup without persistent context
        }
    }

#endif
}
