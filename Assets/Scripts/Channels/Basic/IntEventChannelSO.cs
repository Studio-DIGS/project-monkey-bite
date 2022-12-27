using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Channels/Basic/Int Event Channel")]
public class IntEventChannelSO : DescriptionBaseSO
{
    public Action<int> OnRaised;

    public void RaiseEvent(int val)
    {
        OnRaised?.Invoke(val);
    }
}
