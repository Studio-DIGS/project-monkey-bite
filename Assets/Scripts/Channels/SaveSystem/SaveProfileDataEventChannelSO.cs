using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Channels/SaveSystem/Profile Save Data Event Channel")]
public class SaveProfileDataEventChannelSO : DescriptionBaseSO
{
    public Action<SaveProfileData> OnRaised;

    public void RaiseEvent(SaveProfileData data)
    {
        OnRaised?.Invoke(data);
    }
}