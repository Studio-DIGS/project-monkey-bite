using System;
using UnityEngine;

namespace MushiCore.Editor
{
    public class EditorToolUtils
    {
        /// <summary>
        /// Returns the first instance of some scriptable object exist in the asset database, instantiating a new one if none exists
        /// </summary>
        /// <param name="defaultCreationPath"></param>
        /// <param name="preProcessAction"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetSingletonSettings<T>(string defaultCreationPath, Action<T> preProcessAction) where T : ScriptableObject
        {
            var instance = AssetDatabaseUtils.FindFirstAssetOfType<T>();
            if (instance == null)
            {
                instance = AssetCreateUtility.CreateScriptableObjectDirect<T>(defaultCreationPath, preProcessAction);
            }

            return instance;
        }
    }
}