using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersistentSaveManager : MonoBehaviour
{
    [ColorHeader("Listening - Save Ask Channels", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private ProfileSaveDataEventChannelSO askSaveProfile;
    
    [ColorHeader("Listening - Load Get Channels", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private ProfileSaveDataFuncChannelSO getProfileSave;
    [SerializeField] private ProfileSaveDataArrFuncChannelSO getAllProfileSaves;
    
    [ColorHeader("Save Data SO Containers", ColorHeaderColor.Dependencies)] 
    [SerializeField] private ProfileSaveDataSO permanentSaveDataSO;
    
    [ColorHeader("Save System Dependencies", ColorHeaderColor.Dependencies)] 
    [SerializeField] private SaveIOSO saveIO;
    [SerializeField] private SaveParserSO saveParser;

    private void OnEnable()
    {
        askSaveProfile.OnRaised += SaveProfileDataToFile;
        getProfileSave.OnCalled += LoadProfileDataFromFile;
        getAllProfileSaves.OnCalled += GetAllProfileSaves;
    }

    private void OnDisable()
    {
        askSaveProfile.OnRaised -= SaveProfileDataToFile;
        getProfileSave.OnCalled -= LoadProfileDataFromFile;
        getAllProfileSaves.OnCalled -= GetAllProfileSaves;
    }

    private void SaveProfileDataToFile(ProfileSaveData data)
    {
        Debug.Log($"Saving to profile {data.profileID}");
        string json = saveParser.ProfileSaveDataToJSON(permanentSaveDataSO.profileSaveData);
        saveIO.WriteProfileSaveData(data.profileID, json);
    }

    private ProfileSaveData LoadProfileDataFromFile(string profileID)
    {
        var readSaveData = saveIO.ReadProfileSaveData(profileID);
        ProfileSaveData data = saveParser.JSONToProfileSaveData(readSaveData.saveData);
        return data;
    }

    private ProfileSaveData[] GetAllProfileSaves()
    {
        var jsonData = saveIO.ReadAllProfileSavesData();
        var saveDataObjs = new ProfileSaveData[jsonData.Count];

        for (int i = 0; i < jsonData.Count; i++)
        {
            saveDataObjs[i] = saveParser.JSONToProfileSaveData(jsonData[i].saveData);
        }

        return saveDataObjs;
    }
}
