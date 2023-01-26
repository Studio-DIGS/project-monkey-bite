using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "MushiStuff/MushiEditorTools/Hierarchy Overlays/Hierarchy Overlay Config")]
public class HierarchyOverlayConfigSO : SingleActiveConfigSO
{
    [System.Serializable]
    public struct IconOverlayGroup
    {
        public bool isActive;
        public HierarchyIconOverlayGroupSO group;
    }
    
    [ColorHeader("Separator")] 
    public bool drawSeparators;
    public Color separatorBackgroundColor;
    public Color separatorTextColor;
    public MultiSourceEditorIcon separatorIcon;

    [ColorHeader("Alternating lines")] 
    public bool drawAlternatingLines;
    public Color alternatingLineOverlay;

    [ColorHeader("Nesting Lines")] 
    public bool drawNestingLines;
    public Color nestingLineColor;

    [ColorHeader("Icons")] 
    public bool drawIcons;
    public float iconShrinkAmount;
    public List<IconOverlayGroup> iconOverlays;
}

[CustomEditor(typeof(HierarchyOverlayConfigSO))]
public class HierarchyOverlayConfigDrawer : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        var targetConfig = (HierarchyOverlayConfigSO)target;
        SingleActiveConfigSO.DrawIsActiveField(targetConfig);
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
 