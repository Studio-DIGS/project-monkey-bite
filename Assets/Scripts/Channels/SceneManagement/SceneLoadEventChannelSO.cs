using UnityEngine;
using System;

/// <summary>
/// <param name="T1">Scene to load</param>
/// <param name="T2">Should transition out?</param>
/// <param name="T3">Should transition in?</param>
/// <param name="T4">Load screen actions</param>
/// </summary>
[CreateAssetMenu(menuName = "Channels/Events/SceneManagement/Scene Load Event Channel")]
public class SceneLoadEventChannelSO : GenericEventChannelSO<GameSceneSO, bool, bool, Action>
{
    public override void RaiseEvent(GameSceneSO arg1, bool arg2, bool arg3, Action arg4 = null)
    {
        base.RaiseEvent(arg1, arg2, arg3, arg4);
    }
}
