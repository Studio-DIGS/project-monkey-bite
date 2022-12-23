using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Events/SceneManagement/Scene Unload Event Channel")]
public class SceneUnloadEventChannelSO : DescriptionBaseSO
{
    public Action<bool, bool, Action> OnRaised;

    public void RaiseEvent(bool transitionOut = false, bool transitionIn = false, Action loadScreenActions = null)
    {
        if (OnRaised != null)
        {
            OnRaised?.Invoke(transitionOut, transitionIn, loadScreenActions);
        }
        else
        {
            Debug.LogWarning("A scene unload all was requested, but had no listeners.");
        }
    }
}
