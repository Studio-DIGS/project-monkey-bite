using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Events/SceneManagement/Scene Unload Event Channel")]
public class SceneUnloadAllEventChannelSO : DescriptionBaseSO
{
    public Action OnRaised;

    public void RaiseEvent()
    {
        if (OnRaised != null)
        {
            OnRaised?.Invoke();
        }
        else
        {
            Debug.LogWarning("A scene unload all was requested, but had no listeners.");
        }
    }
}
