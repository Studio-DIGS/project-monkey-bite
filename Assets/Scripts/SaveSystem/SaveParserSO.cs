using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/SaveSystem/SaveParser")]
public class SaveParserSO : DescriptionBaseSO
{
    public string ProfileSaveDataToJSON(ProfileSaveData data)
    {
        return JsonUtility.ToJson(data);
    }

    public ProfileSaveData JSONToProfileSaveData(string jsonData)
    {
        return JsonUtility.FromJson<ProfileSaveData>(jsonData);
    }
}
