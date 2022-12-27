using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Channels/SaveSystem/Profile Save Data Function Channel")]
public class ProfileSaveDataFuncChannelSO : DescriptionBaseSO
{
    public Func<string, ProfileSaveData> OnCalled;

    public ProfileSaveData CallFunc(string profileName)
    {
        return OnCalled?.Invoke(profileName);
    }
}