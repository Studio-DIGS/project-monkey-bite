using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Events/SceneManagement/Scene Unload Event Channel")]
public class SceneUnloadEventChannelSO : DescriptionBaseSO
{
    public Action<bool, bool> OnRaised;

    public void RaiseEvent(bool transitionOut = false, bool transitionIn = false)
    {
        if (OnRaised != null)
        {
            OnRaised?.Invoke(transitionOut, transitionIn);
        }
        else
        {
            Debug.LogWarning("A scene unload all was requested, but had no listeners.");
        }
    }
}
