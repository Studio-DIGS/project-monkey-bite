using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/SaveSystem/ProfileSaveDataSO")]
public class ProfileSaveDataSO : DescriptionBaseSO
{
    public ProfileSaveData profileSaveData;
}

[System.Serializable]
public class ProfileSaveData
{
    [ColorHeader("Save Profile Meta Data")] 
    public int profileIndex;
    public string profileID;
    
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
