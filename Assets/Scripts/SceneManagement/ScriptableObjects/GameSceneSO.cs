using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameSceneSO : DescriptionBaseSO
{
    public GameSceneType sceneType;
    public AssetReference sceneReference; //Loading scene from asset bundle

    /// <summary>
    /// Scene type
    /// </summary>
    public enum GameSceneType
    {
        Initialization,
        PersistentManagers,
        GameplayContent,
        GameplayManager,
        MainMenuManager,
        MainMenuContent
    }
}
