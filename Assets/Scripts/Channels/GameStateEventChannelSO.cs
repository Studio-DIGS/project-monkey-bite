using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Channels/Game State Event Channel")]
public class GameStateEventChannelSO : DescriptionBaseSO
{
    public Action<GameState> OnRaised;

    public void RaiseEvent(GameState state)
    {
        if (OnRaised != null)
        {
            OnRaised?.Invoke(state);
        }
        else
        {
            Debug.LogWarning("A GameState change was requested but had no listeners");
        }
    }
}
