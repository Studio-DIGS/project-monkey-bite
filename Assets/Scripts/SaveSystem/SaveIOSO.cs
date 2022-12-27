using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Directory = UnityEngine.Windows.Directory;
using File = UnityEngine.Windows.File;

[CreateAssetMenu(menuName = "Architecture/SaveSystem/SaveRetriever")]
public class SaveIOSO : ScriptableObject
{
    public RelativeSavePathConfigSO pathConfig;
    
    /// <summary>
    /// Retrives any permanent save data (unlocks, achievements, etc) on a profile
    /// </summary>
    /// <param name="profileIndex"></param>
    /// <returns></returns>
    public ReadSaveData ReadPermanentSaveData(int profileIndex)
    {
        var fullPath = GetFullPath(pathConfig.GetProfileSaveRelativePath("" + profileIndex));
        return ReadStringData(fullPath);
    }

    public void WritePermanentSaveData(int profileIndex, string data)
    {
        var fullPath = GetFullPath(pathConfig.GetProfileSaveRelativePath("" + profileIndex));
        WriteStringData(fullPath, data);
    }

    private string GetFullPath(string relativePath)
    {
        return Application.persistentDataPath + '/' + relativePath;
    }

    private ReadSaveData ReadStringData(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                var result = File.ReadAllBytes(filePath);
                string data = System.Text.Encoding.Default.GetString(result);
                return new ReadSaveData(data, true);
            }
            else
            {
                Debug.LogWarning($"Unable to find file at {filePath}, returning empty string save data");
                return new ReadSaveData(String.Empty, false);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    private void WriteStringData(string filePath, string stringData)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            var byteData = System.Text.Encoding.Default.GetBytes(stringData);
            File.WriteAllBytes(filePath, byteData);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }
}

public struct ReadSaveData
{
    public readonly bool saveDataFound;
    public readonly string saveData;

    public ReadSaveData(string saveData, bool saveDataFound)
    {
        this.saveData = saveData;
        this.saveDataFound = saveDataFound;
    }
}


