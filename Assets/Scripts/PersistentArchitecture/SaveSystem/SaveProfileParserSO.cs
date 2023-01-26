using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/SaveSystem/SaveParser")]
public class SaveProfileParserSO : DescriptionBaseSO
{
    public string ProfileSaveDataToJSON(SaveProfileData data)
    {
        return JsonUtility.ToJson(data);
    }

    public SaveProfileData JSONToProfileSaveData(string jsonData)
    {
        return JsonUtility.FromJson<SaveProfileData>(jsonData);
    }
}
