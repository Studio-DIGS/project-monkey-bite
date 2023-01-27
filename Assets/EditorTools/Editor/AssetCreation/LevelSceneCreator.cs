using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MushiEditorTools.AssetCreationUtils;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class LevelSceneCreator
{
    static readonly string resourcePath = "Assets/Scenes/Templates";
    
    [MenuItem("Assets/Create/SceneCreation/New Gameplay Level", priority = 1000)]
    public static void CreateGameplayLevel()
    {
        // Create the template scene
        string templatePath = $"{resourcePath}/GameLevelTemplate.unity";
        var onCreated = AssetTemplateUtility.CreateAssetFileFromTemplate(
            templatePath,
            "NewGameLevel.unity",
                        OnCreateScene
            );

        void OnCreateScene(string sceneFilePath)
        {
            Debug.Log($"Creating a new Gameplay Level Scene from template at {sceneFilePath} using the template at {templatePath}");
            string folderPath = Path.GetDirectoryName(sceneFilePath);
            string levelName = Path.GetFileName(sceneFilePath).Split('.')[0];

            string sceneAssetGuid = AssetDatabase.GUIDFromAssetPath(sceneFilePath).ToString();

            void PreProcessSO(GameplayLevelSceneSO levelScene)
            {
                levelScene.sceneType = GameSceneSO.GameSceneType.GameplayContent;
                levelScene.sceneReference = new AssetReference(sceneAssetGuid);
            }
            
            AssetCreateUtility.CreateScriptableObjectDirect<GameplayLevelSceneSO>(
                $"{folderPath}/{levelName}.asset",
                PreProcessSO
            );
        }
    }
}

