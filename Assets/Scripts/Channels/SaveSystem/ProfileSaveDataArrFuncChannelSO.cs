using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Channels/SaveSystem/Profile Save Data Array Function Channel")]
public class ProfileSaveDataArrFuncChannelSO : DescriptionBaseSO
{
    public Func<ProfileSaveData[]> OnCalled;

    public ProfileSaveData[] CallFunc()
    {
        return OnCalled?.Invoke();
    }
}