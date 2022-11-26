using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Events/SceneManagement/Scene Unload Event Channel")]
public class SceneUnloadAllEventChannelSO : DescriptionBaseSO
{
    public Action OnUnloadingRequested;

    public void RaiseEvent()
    {
        if (OnUnloadingRequested != null)
        {
            OnUnloadingRequested?.Invoke();
        }
        else
        {
            Debug.LogWarning("A scene unload all was requested, but had no listeners.");
        }
    }
}
