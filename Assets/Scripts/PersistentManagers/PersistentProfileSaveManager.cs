using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Persistent manager acting as the interface for saving and loading save profiles
/// </summary>
public class PersistentProfileSaveManager : DescriptionMonoBehavior
{
    [ColorHeader("Listening - Save Ask Channels", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private SaveProfileDataEventChannelSO askSaveProfile;
    
    [ColorHeader("Listening - Load Get Channels", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private SaveProfileDataFuncChannelSO getSaveProfile;
    [SerializeField] private SaveProfileDataArrFuncChannelSO getAllSaveProfile;

    [ColorHeader("Listening - Set Active Profile Save Ask Channel", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private SaveProfileDataEventChannelSO askSetActiveSaveProfile;
    
    [ColorHeader("Save Data SO Containers", ColorHeaderColor.Dependencies)] 
    [SerializeField] private ProfileSaveDataSO activeProfileContainer;
    
    [ColorHeader("Save System Dependencies", ColorHeaderColor.Dependencies)] 
    [SerializeField] private SaveProfileIOSO saveProfileIO;
    [SerializeField] private SaveProfileParserSO saveProfileParser;

    private void OnEnable()
    {
        askSaveProfile.OnRaised += SaveProfileDataToFile;
        getSaveProfile.OnCalled += LoadProfileDataFromFile;
        getAllSaveProfile.OnCalled += GetAllProfileSaves;
        askSetActiveSaveProfile.OnRaised += SetActiveProfile;
    }

    private void OnDisable()
    {
        askSaveProfile.OnRaised -= SaveProfileDataToFile;
        getSaveProfile.OnCalled -= LoadProfileDataFromFile;
        getAllSaveProfile.OnCalled -= GetAllProfileSaves;
        askSetActiveSaveProfile.OnRaised -= SetActiveProfile;
    }

    private void SetActiveProfile(SaveProfileData data)
    {
        activeProfileContainer.saveProfileData = data;
    }

    private void SaveProfileDataToFile(SaveProfileData data)
    {
        if (data.metaData.doNotSave)
        {
            Debug.Log($"Profile '{data.metaData.profileID}' marked do not save, skipping");
            return;
        }
        Debug.Log($"Saving to profile '{data.metaData.profileID}'");
        string json = saveProfileParser.ProfileSaveDataToJSON(data);
        saveProfileIO.WriteProfileSaveData(data.metaData.profileID, json);
    }

    private SaveProfileData LoadProfileDataFromFile(string profileID)
    {
        var readSaveData = saveProfileIO.ReadProfileSaveData(profileID);
        SaveProfileData data = saveProfileParser.JSONToProfileSaveData(readSaveData.saveData);
        return data;
    }

    private SaveProfileData[] GetAllProfileSaves()
    {
        var jsonData = saveProfileIO.ReadAllProfileSavesData();
        var saveDataObjs = new SaveProfileData[jsonData.Count];

        for (int i = 0; i < jsonData.Count; i++)
        {
            saveDataObjs[i] = saveProfileParser.JSONToProfileSaveData(jsonData[i].saveData);
        }

        return saveDataObjs;
    }
}
