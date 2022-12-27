using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSaveManager : MonoBehaviour
{
    [ColorHeader("Listening - Save ask Channels", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private IntEventChannelSO askSavePermanentDataFromSO;
    
    [ColorHeader("Listening - Load ask Channels", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private IntEventChannelSO askLoadPermanentDataToSO;
    
    [ColorHeader("Save Data SO Containers", ColorHeaderColor.Dependencies)] 
    [SerializeField] private ProfileSaveDataSO permanentSaveDataSO;
    
    [ColorHeader("Save System Dependencies", ColorHeaderColor.Dependencies)] 
    [SerializeField] private SaveIOSO saveIO;
    [SerializeField] private SaveParserSO saveParser;

    private void OnEnable()
    {
        askSavePermanentDataFromSO.OnRaised += SavePermanentDataFromSO;
        askLoadPermanentDataToSO.OnRaised += LoadPermanentDataFromSO;
    }

    private void OnDisable()
    {
        askSavePermanentDataFromSO.OnRaised -= SavePermanentDataFromSO;
        askLoadPermanentDataToSO.OnRaised -= LoadPermanentDataFromSO;
    }

    private void SavePermanentDataFromSO(int profileIndex)
    {
        string json = saveParser.PermanentSaveDataToJSON(permanentSaveDataSO.loadedData);
        saveIO.WritePermanentSaveData(profileIndex, json);
    }

    private void LoadPermanentDataFromSO(int profileIndex)
    {
        var readSaveData = saveIO.ReadPermanentSaveData(profileIndex);
        ProfileSaveData data = saveParser.JSONToPermanentSaveData(readSaveData.saveData);
        permanentSaveDataSO.loadedData = data ?? new ProfileSaveData();
    }
}
