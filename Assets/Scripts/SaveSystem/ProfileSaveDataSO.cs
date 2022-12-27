using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/SaveSystem/ProfileSaveDataSO")]
public class ProfileSaveDataSO : ScriptableObject
{
    public ProfileSaveData loadedData;
}

[System.Serializable]
public class ProfileSaveData
{
    [ColorHeader("Save Profile Meta Data")] 
    public int profileIndex;
    public string profileName;
    
    [ColorHeader("Permanent Data")]
    public BasicStatsSaveData statsData;

    [ColorHeader("Run Data")]
    public bool isRunInProgress;
    public RunSaveData runData;

    public ProfileSaveData()
    {
        runData = new RunSaveData();
        statsData = new BasicStatsSaveData();
    }
}
