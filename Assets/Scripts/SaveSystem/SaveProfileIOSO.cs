using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Directory = UnityEngine.Windows.Directory;
using File = UnityEngine.Windows.File;

[CreateAssetMenu(menuName = "Architecture/SaveSystem/SaveRetriever")]
public class SaveProfileIOSO : DescriptionBaseSO
{
    public RelativeSavePathConfigSO pathConfig;
    
    /// <summary>
    /// Retrives any permanent save data (unlocks, achievements, etc) on a profile
    /// </summary>
    /// <param name="profileID"></param>
    /// <returns></returns>
    public ReadSaveData ReadProfileSaveData(string profileID)
    {
        var fullPath = GetFullPath(pathConfig.GetProfileSaveRelativePath(profileID));
        return ReadStringData(fullPath);
    }

    public void WriteProfileSaveData(string profileID, string data)
    {
        var fullPath = GetFullPath(pathConfig.GetProfileSaveRelativePath(profileID));
        WriteStringData(fullPath, data);
    }

    private string GetFullPath(string relativePath)
    {
        return Application.persistentDataPath + '/' + relativePath;
    }

    public List<ReadSaveData> ReadAllProfileSavesData()
    {
        string directoryPath = GetFullPath(pathConfig.RelativeDirectory);
        string postFix = pathConfig.FileTypePostfix;

        Debug.Log($"Reading all save data from {directoryPath}");
        
        var filePaths = System.IO.Directory.GetFiles(directoryPath).Where(file => file.EndsWith(postFix)).ToList();
        var results = new List<ReadSaveData>();

        foreach (var path in filePaths)
        {
            results.Add(ReadStringData(path));
        }

        return results;
    }

    private ReadSaveData ReadStringData(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                var result = File.ReadAllBytes(filePath);
                string data = BytesToString(result);
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
            var byteData = StringToBytes(stringData);
            File.WriteAllBytes(filePath, byteData);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }
    
    private string BytesToString(byte[] data)
    {
        return System.Text.Encoding.Default.GetString(data);
    }

    private byte[] StringToBytes(string str)
    {
        return System.Text.Encoding.Default.GetBytes(str);
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


