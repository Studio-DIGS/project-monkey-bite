using System;
using System.Collections;
using System.Collections.Generic;
using MushiCore.EditorAttributes;
using UnityEditor;
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
    [SerializeField] private GameSceneSO persistentManagerSceneData;
    
    [ColorHeader("Writing - Cold Startup Data", ColorHeaderColor.WritingState)]
    [SerializeField] private ColdStartupDataSO coldStartupData;
    
    private bool isColdStart = false;
    private GameSceneSO thisSceneData;

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
            FindThisSceneData();
            // First load Persistent Managers scene
            coldStartupData.isColdStartup = true;
            persistentManagerSceneData.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, false).Completed += OnPersistentManagersLoaded;
        }
    }

    private void FindThisSceneData()
    {
        var guids = AssetDatabase.FindAssets("t:" + typeof(GameSceneSO).ToString());
        var loadedScene = SceneManager.GetActiveScene();
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<GameSceneSO>(path);
            if (loadedScene.path == AssetDatabase.GUIDToAssetPath(asset.sceneReference.AssetGUID))
            {
                thisSceneData = asset;
                break;
            }
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
            Debug.LogError("No Game Scene Data found for cold startup");
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
    

#endif
}