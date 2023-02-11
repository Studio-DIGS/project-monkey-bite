using UnityEditor;
using UnityEngine;

namespace MushiCore.Editor
{
    public static class AssetDatabaseUtils
    {
        public static T[] FindAllAssetsOfType<T>() where T : Object
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            var result = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                var guid = guids[i];
                string path = AssetDatabase.GUIDToAssetPath(guid);
                result[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return result;
        }

        public static T FindFirstAssetOfType<T>() where T : Object
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            if (guids.Length == 0)
            {
                return null;
            }
            else
            {
                return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guids[0]));
            }
        }
    }
}