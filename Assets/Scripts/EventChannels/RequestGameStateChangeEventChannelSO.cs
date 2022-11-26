using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Events/Request Gamestate Change Event Channel")]
public class RequestGameStateChangeEventChannelSO : DescriptionBaseSO
{
    public Action<GameState> OnRequestGameStateChange;

    public void RaiseEvent(GameState state)
    {
        if (OnRequestGameStateChange != null)
        {
            OnRequestGameStateChange?.Invoke(state);
        }
        else
        {
            Debug.LogWarning("A GameState change was requested but had no listeners");
        }
    }
}
