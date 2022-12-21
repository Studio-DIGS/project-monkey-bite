using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A group of HierarchyIconOverlays, used to organize configs
/// </summary>
[CreateAssetMenu(menuName = "MushiTools/Hierarchy Overlays/Icons/Overlay Group")]
public class HierarchyIconOverlayGroupSO : ScriptableObject
{
    public List<HierarchyIconStringTargetSO> StringTargetIcons;
    public List<HierarchyIconMonoScriptTargetSO> MonoScriptTargetIcons;
    
    public Dictionary<string, HierarchyIconStringTargetSO> StringTargetDict;
    
    private void OnValidate()
    {
        StringTargetDict = new Dictionary<string, HierarchyIconStringTargetSO>();
        foreach (var stringTarget in StringTargetIcons)
        {
            if(!stringTarget) continue;
            StringTargetDict.TryAdd(stringTarget.targetClassString.ToLower(), stringTarget);
        }
    }
}
