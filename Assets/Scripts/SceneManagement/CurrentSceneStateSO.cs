using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

[CreateAssetMenu(menuName="Architecture/SceneManagement/SceneStateSO")]
public class CurrentSceneStateSO : DescriptionBaseSO
{
    // Manager Scenes
    public GameSceneSO currentlyLoadedManagerScene;
    
    // Content Scenes
    public GameSceneSO currentlyLoadedContentScene;
}
