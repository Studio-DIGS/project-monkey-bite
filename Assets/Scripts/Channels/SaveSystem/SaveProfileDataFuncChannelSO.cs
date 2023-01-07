using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Channels/SaveSystem/Profile Save Data Function Channel")]
public class SaveProfileDataFuncChannelSO : DescriptionBaseSO
{
    public Func<string, SaveProfileData> OnCalled;

    public SaveProfileData CallFunc(string profileName)
    {
        return OnCalled?.Invoke(profileName);
    }
}