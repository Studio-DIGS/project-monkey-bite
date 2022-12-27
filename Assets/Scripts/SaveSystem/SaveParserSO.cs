using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/SaveSystem/SaveParser")]
public class SaveParserSO : ScriptableObject
{
    public string PermanentSaveDataToJSON(ProfileSaveData data)
    {
        return JsonUtility.ToJson(data);
    }

    public ProfileSaveData JSONToPermanentSaveData(string jsonData)
    {
        return JsonUtility.FromJson<ProfileSaveData>(jsonData);
    }
}
