using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Channels/SaveSystem/Profile Save Data Event Channel")]
public class ProfileSaveDataEventChannelSO : DescriptionBaseSO
{
    public Action<ProfileSaveData> OnRaised;

    public void RaiseEvent(ProfileSaveData data)
    {
        OnRaised?.Invoke(data);
    }
}