using System;
using System.Collections;
using System.Collections.Generic;
using MushiCore.EditorAttributes;
using UnityEngine;

[System.Serializable]
public class SaveProfileData
{
    [ColorHeader("Save Profile Meta Data")]
    public SaveProfileMetaData metaData;

    
    [ColorHeader("Permanent Data")]
    public BasicStatsSaveData statsData;

    [ColorHeader("Run Data")]
    public RunSaveData runData;

    public SaveProfileData()
    {
        metaData = new SaveProfileMetaData();
        runData = new RunSaveData();
        statsData = new BasicStatsSaveData();
    }
}

[System.Serializable]
public class SaveProfileMetaData
{
    public bool doNotSave;
    public string profileID;
}

[System.Serializable]
public class BasicStatsSaveData
{
    public int deathCounter = 0;
    public float playTime;
}

[System.Serializable]
public class RunSaveData
{
    public bool isRunInProgress;
    public int levelsCleared;
}
