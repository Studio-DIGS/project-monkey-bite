using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Events/Basic/VoidChannel")]
public class VoidEventChannelSO : DescriptionBaseSO
{
    public Action OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}
