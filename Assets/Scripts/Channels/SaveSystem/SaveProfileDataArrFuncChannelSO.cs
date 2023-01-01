using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Channels/SaveSystem/Profile Save Data Array Function Channel")]
public class SaveProfileDataArrFuncChannelSO : DescriptionBaseSO
{
    public Func<SaveProfileData[]> OnCalled;

    public SaveProfileData[] CallFunc()
    {
        return OnCalled?.Invoke();
    }
}