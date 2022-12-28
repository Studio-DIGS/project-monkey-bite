using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/SceneManagement/ColdStartupDataSO")]
public class ColdStartupDataSO : DescriptionBaseSO
{
    enum ColdStartupLoadType
    {
        CustomProfileDataInstance,
        LoadFromProfileID
    }

    [ColorHeader("Cold Startup Configuration", ColorHeaderColor.Config)]
    [SerializeField] private ColdStartupLoadType coldStartupLoadType;
    [SerializeField] private ProfileSaveDataSO coldStartupSaveProfile;
    [SerializeField] private string profileID;
    [SerializeField] private ProfileSaveDataFuncChannelSO getProfileSaveData;
    [SerializeField] private ProfileSaveDataEventChannelSO askSetActiveProfileSave;

    [ColorHeader("Readonly - State provided by EditorColdStartup")]
    [ReadOnly, DoNotSerialize] public GameSceneSO startupScene;
    [ReadOnly, DoNotSerialize] public bool isColdStartup;

    public void SetColdStartupSaveProfileActive()
    {
        if (coldStartupLoadType == ColdStartupLoadType.CustomProfileDataInstance)
        {
            askSetActiveProfileSave.RaiseEvent(coldStartupSaveProfile.profileSaveData);
        }
        else
        {
            askSetActiveProfileSave.RaiseEvent(getProfileSaveData.CallFunc(profileID));
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
