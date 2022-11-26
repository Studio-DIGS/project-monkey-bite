using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Events/SceneManagement/Scene Cold Startup Channel")]
public class ColdStartupEventChannelSO : DescriptionBaseSO
{
    public Action<GameSceneSO, bool> OnColdStartup;

    /// <summary>
    /// Raise cold startup event, must specify if manager level scene or content level
    /// </summary>
    /// <param name="startupScene"></param>
    /// <param name="isContentScene"></param>
    public void RaiseEvent(GameSceneSO startupScene, bool isContentScene)
    {
        if (OnColdStartup != null)
        {
            OnColdStartup?.Invoke(startupScene, isContentScene);
        }
        else
        {
            Debug.LogWarning("A cold startup was raised, but had no listeners");
        }
    }
}
