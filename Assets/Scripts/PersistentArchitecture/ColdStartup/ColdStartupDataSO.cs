using System.Collections;
using System.Collections.Generic;
using MushiCore.EditorAttributes;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/SceneManagement/ColdStartupDataSO")]
public class ColdStartupDataSO : DescriptionBaseSO
{
    enum ColdStartupSaveProfileType
    {
        LoadCustomInstance,
        LoadFromProfileID
    }

    [ColorHeader("Cold Startup Save Profile Config", ColorHeaderColor.Config)]
    [SerializeField] private ColdStartupSaveProfileType coldStartupSaveProfileType;
    [SerializeField] private ProfileSaveDataSO customSaveProfile;
    [SerializeField] private string profileID;
    
    [ColorHeader("Invoking", ColorHeaderColor.InvokingChannels)]
    [SerializeField] private SaveProfileDataFuncChannelSO getSaveProfileData;
    [SerializeField] private SaveProfileDataEventChannelSO askSetActiveSaveProfile;

    [ColorHeader("Readonly - State provided by EditorColdStartup")]
    [EditorReadOnly, DoNotSerialize] public GameSceneSO startupScene;
    [EditorReadOnly, DoNotSerialize] public bool isColdStartup;

    public void SetColdStartupSaveProfileActive()
    {
        if (coldStartupSaveProfileType == ColdStartupSaveProfileType.LoadCustomInstance)
        {
            askSetActiveSaveProfile.RaiseEvent(customSaveProfile.saveProfileData);
        }
        else
        {
            askSetActiveSaveProfile.RaiseEvent(getSaveProfileData.CallFunc(profileID));
        }
    }

    /// <summary>
    /// Clears cold startup. Manager-level scripts should consume the cold startup once it is finished loading.
    /// This is done to prevent cold startups from occuring multiple times in a single play session
    /// </summary>
    public void ConsumeColdStartup()
    {
        startupScene = null;
        isColdStartup = false;
    }
}
