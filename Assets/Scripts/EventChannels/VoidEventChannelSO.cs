using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Events/Basic/Void Event Channel")]
public class VoidEventChannelSO : DescriptionBaseSO
{
    public Action OnRaised;

    public void RaiseEvent()
    {
        OnRaised?.Invoke();
    }
}
