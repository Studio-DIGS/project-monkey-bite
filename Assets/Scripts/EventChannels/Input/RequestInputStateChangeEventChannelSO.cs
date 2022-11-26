using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Events/Request Input State ChangeChannel")]
public class RequestInputStateChangeEventChannelSO : DescriptionBaseSO
{
    public Action<InputState> OnRequestInputStateChange;

    public void RaiseEvent(InputState state)
    {
        if (OnRequestInputStateChange != null)
        {
            OnRequestInputStateChange?.Invoke(state);
        }
        else
        {
            Debug.LogWarning("An InputState change was requested but had no listeners");
        }
    }
}
