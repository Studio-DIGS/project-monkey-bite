using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Events/Input State Channel")]
public class InputStateEventChannelSO : DescriptionBaseSO
{
    public Action<InputState> OnRaised;

    public void RaiseEvent(InputState state)
    {
        if (OnRaised != null)
        {
            OnRaised?.Invoke(state);
        }
        else
        {
            Debug.LogWarning("An InputState Event Channel was raised but had no listeners");
        }
    }
}
