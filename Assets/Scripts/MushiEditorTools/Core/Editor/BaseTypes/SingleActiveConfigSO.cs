using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Scriptable object in which only one instance can be "active" at a time
/// Used for editor tools which use a config SO (color header , for example)
/// These should always be in 'Editor' folders
/// </summary>
public abstract class SingleActiveConfigSO : ScriptableObject
{
    // Subclasses should have a custom drawer for this that uses SetActiveConfig()
    [HideInInspector, SerializeField] private bool isActiveConfig;
    public bool IsActiveConfig => isActiveConfig;

    // Event dictionary for updating tools when active config changes
    private static Dictionary<Type, Action> activeConfigChangedEvents = new Dictionary<Type, Action>();

    private void OnValidate()
    {
        // Roundabout way of dealing with duplicating SOs
        // Prevents both duplicate and source objects from both being active
        SetActiveConfig(isActiveConfig);
    }

    /// <summary>
    /// Get all config SOs of type name
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public static SingleActiveConfigSO[] GetAllConfigs(string typeName)
    {
        var guids = AssetDatabase.FindAssets($"t:{typeName}");
        var results = new SingleActiveConfigSO[guids.Length];
        for(int i = 0;i < guids.Length;i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            results[i] = AssetDatabase.LoadAssetAtPath<SingleActiveConfigSO>(path);
        }
        return results;
    }
    
    /// <summary>
    /// Get all configs SOs of generic type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T[] GetAllConfigs<T>() where T : SingleActiveConfigSO
    {
        return GetAllConfigs(typeof(T).ToString()).Select(config => (T)config).ToArray();
    }

    /// <summary>
    /// Get the active config SO using type name
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public static SingleActiveConfigSO GetActiveConfig(string typeName)
    {
        return GetAllConfigs(typeName).FirstOrDefault(config => config.IsActiveConfig);
    }
    
    /// <summary>
    /// Get the active config SO of generic type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetActiveConfig<T>() where T : SingleActiveConfigSO
    {
        return (T)GetAllConfigs(typeof(T).ToString()).FirstOrDefault(config => config.IsActiveConfig);
    }
    
    /// <summary>
    /// Set this config to be active, automatically setting the previously active config to be inactive
    /// </summary>
    /// <param name="active"></param>
    public void SetActiveConfig(bool active)
    {
        var type = GetType();
        if (active)
        {
            var prevActiveConfig = GetActiveConfig(type.ToString());
            if (prevActiveConfig != null)
            {
                prevActiveConfig.isActiveConfig = false;
            }
            isActiveConfig = true;
        }
        else
        {
            isActiveConfig = false;
        }
                                        
        if (activeConfigChangedEvents.ContainsKey(type))
        {
            activeConfigChangedEvents[type]?.Invoke();
        } 
    }

    /// <summary>
    /// Add a listener for active config change events
    /// </summary>
    public static void AddConfigChangeListener<T>(Action listener) where T : SingleActiveConfigSO
    {
        var type = typeof(T);
        if (!activeConfigChangedEvents.ContainsKey(type))
        {
            activeConfigChangedEvents.TryAdd(typeof(T), listener);
        }
        else
        {
            activeConfigChangedEvents[type] += listener;
        }
    }
    
    /// <summary>
    /// Remove a listener for active config change events
    /// </summary>
    public static void RemoveConfigChangeListener<T>(Action listener) where T : SingleActiveConfigSO
    {
        var type = typeof(T);
        if (activeConfigChangedEvents.ContainsKey(type))
        {
            activeConfigChangedEvents[type] -= listener;
        }
    }
    
    /// <summary>
    /// Manually trigger the config changed event for some config type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void TriggerConfigChangeEvent<T>() where T : SingleActiveConfigSO
    {
        var type = typeof(T);
        if (activeConfigChangedEvents.ContainsKey(type))
        {
            activeConfigChangedEvents[type]?.Invoke();
        }
    }

    /// <summary>
    /// Utility function for drawing isActive field, since requires special handling due to affecting other config objects
    /// </summary>
    /// <param name="targetConfig"></param>
    public static void DrawSingleActiveConfigSOField(SingleActiveConfigSO targetConfig)
    {
        bool prevValue = targetConfig.isActiveConfig;
        
        bool newVal = EditorGUILayout.Toggle("Is Active Config", prevValue);

        if (prevValue != newVal)
        {
            EditorUtility.SetDirty(targetConfig);
            targetConfig.SetActiveConfig(newVal);
        }
    }
}
