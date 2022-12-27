using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Channels/Basic/Void Event Channel")]
public class VoidEventChannelSO : DescriptionBaseSO
{
    public Action OnRaised;

    public void RaiseEvent()
    {
        OnRaised?.Invoke();
    }
}
