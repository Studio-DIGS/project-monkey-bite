using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Channels/SceneManagement/Scene Load Event Channel")]
public class SceneLoadEventChannelSO : DescriptionBaseSO
{
    public Action<GameSceneSO, bool, bool, Action> OnRaised;

    public void RaiseEvent(GameSceneSO locationToLoad,  bool transitionOut = false, bool transitionIn = false, Action loadScreenActions = null)
    {
        if (OnRaised != null)
        {
            OnRaised?.Invoke(locationToLoad, transitionOut, transitionIn, loadScreenActions);
        }
        else
        {
            Debug.LogWarning("A Scene loading was requested, but nobody picked it up. " +
                             "Check why there is no SceneLoader already present, " +
                             "and make sure it's listening on this Load Event channel.");
        }
    }
}
