using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "MushiTools/Overlays/HierarchyOverlayConfig")]
public class HierarchyOverlayConfigSO : SingleActiveConfigSO
{
    [ColorHeader("Separator")]
    public Color SeparatorBackgroundColor;
    public Color SeparatorTextColor;
    public MultiSourceEditorIcon SeparatorIcon;

    [ColorHeader("Alternating lines")] 
    public Color AlternatingLineOverlay;

    [ColorHeader("Nesting Lines")] 
    public Color NestingLineColor;

    [ColorHeader("Icons")] 
    public float iconShrinkAmount;
    public List<HierarchyIconOverlay> IconOverlays;
}

[CustomEditor(typeof(HierarchyOverlayConfigSO))]
public class HierarchyOverlayConfigDrawer : Editor
{
    public override void OnInspectorGUI() 
    {
        var so = new SerializedObject(target);
        var targetConfig = ((HierarchyOverlayConfigSO)target);
        var activeField = so.FindProperty("isActiveConfig");
        Undo.RecordObject(targetConfig, "Hierarchy Overlay Config Change");
        bool val = EditorGUILayout.Toggle(activeField.displayName, activeField.boolValue);
        targetConfig.SetActiveConfig(val);
        base.OnInspectorGUI();
    }
}
 