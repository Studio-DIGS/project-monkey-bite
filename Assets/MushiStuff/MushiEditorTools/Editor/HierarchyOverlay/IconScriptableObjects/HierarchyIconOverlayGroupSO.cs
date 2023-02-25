using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MushiEditorTools.HierarchyOverlay
{
    /// <summary>
    /// A group of HierarchyIconOverlays, used to organize configs
    /// </summary>
    [CreateAssetMenu(menuName = "MushiStuff/MushiEditorTools/Hierarchy Overlays/Icon Group")]
    public class HierarchyIconOverlayGroupSO : ScriptableObject
    {
        public List<HierarchyIconSO> hierarchyIcons;

        public Dictionary<string, HierarchyIconSO> stringTargetDict;
        public Dictionary<MonoScript, HierarchyIconSO> monoScriptTargetDict;

        private void OnValidate()
        {
            UpdateDict();
        }

        public void UpdateDict()
        {
            // Build up dictionary for targets for performance
            stringTargetDict = new Dictionary<string, HierarchyIconSO>();
            monoScriptTargetDict = new Dictionary<MonoScript, HierarchyIconSO>();
            
            foreach (var target in hierarchyIcons)
            {
                if (!target) continue;
                foreach (var stringTarget in target.targetClassStrings)
                {
                    if (!stringTargetDict.TryAdd(stringTarget.ToLower(), target))
                    {
                        Debug.Log($"Duplicate icon target {stringTarget}", target);
                    }
                }

                foreach (var monoScriptTarget in target.targetClassMonoscripts)
                {
                    if (!monoScriptTargetDict.TryAdd(monoScriptTarget, target))
                    {
                        Debug.Log($"Duplicate icon target {monoScriptTarget.name}", target);
                    }
                }
            }
        }
    }
}