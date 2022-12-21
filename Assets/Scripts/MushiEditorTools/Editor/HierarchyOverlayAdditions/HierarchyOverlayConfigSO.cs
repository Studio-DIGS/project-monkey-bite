using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "MushiTools/Hierarchy Overlays/Hierarchy Overlay Config")]
public class HierarchyOverlayConfigSO : SingleActiveConfigSO
{
    [System.Serializable]
    public struct IconOverlayGroup
    {
        public bool isActive;
        public HierarchyIconOverlayGroupSO group;
    }
    
    [ColorHeader("Separator")] 
    public bool DrawSeparators;
    public Color SeparatorBackgroundColor;
    public Color SeparatorTextColor;
    public MultiSourceEditorIcon SeparatorIcon;

    [ColorHeader("Alternating lines")] 
    public bool DrawAlternatingLines;
    public Color AlternatingLineOverlay;

    [ColorHeader("Nesting Lines")] 
    public bool DrawNestingLines;
    public Color NestingLineColor;

    [ColorHeader("Icons")] 
    public bool DrawIcons;
    public float iconShrinkAmount;
    public List<IconOverlayGroup> IconOverlays;
}

[CustomEditor(typeof(HierarchyOverlayConfigSO))]
public class HierarchyOverlayConfigDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        var targetConfig = (HierarchyOverlayConfigSO)target;
        SingleActiveConfigSO.DrawSingleActiveConfigSOField(targetConfig);
        if (targetConfig.IsActiveConfig)
        {
            if (GUILayout.Button("Refresh Hierarchy Overlay"))
            {
                SingleActiveConfigSO.TriggerConfigChangeEvent<HierarchyOverlayConfigSO>();
            }
        }
        base.OnInspectorGUI();
    }
}
 