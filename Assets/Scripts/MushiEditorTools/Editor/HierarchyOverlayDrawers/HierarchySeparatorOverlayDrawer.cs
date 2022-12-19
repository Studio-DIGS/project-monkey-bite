using System;
using UnityEditor;
using UnityEngine;

public class HierarchySeparatorOverlay : HierarchyItemOverlay
{
    private GUIStyle overlayStyle;
    private GUIContent iconContent;
    
    public HierarchySeparatorOverlay()
    {
        overlayStyle = new GUIStyle(EditorStyles.label);
        overlayStyle.alignment = TextAnchor.MiddleLeft;
    }

    public override void DrawHierarchyItem(GameObject gameObject, Rect selectionRect, HierarchyOverlayConfigSO config)
    {
        selectionRect.x = 0; 
        selectionRect.width = EditorGUIUtility.currentViewWidth;
        
        // Background
        EditorGUI.DrawRect(selectionRect, config.SeparatorBackgroundColor);
                
        // Icon
        iconContent = config.SeparatorIcon;
        
        // Text style
        overlayStyle.normal.textColor = config.SeparatorTextColor;
        overlayStyle.hover.textColor = config.SeparatorTextColor;
        
        // Setup text
        string displayName = gameObject.name.Trim('/');
        iconContent.text = displayName;

        EditorGUI.LabelField(selectionRect, iconContent, overlayStyle);
        
        // Scuffed disabling
        gameObject.isStatic = true;
        gameObject.SetActive(false);
    }

    public override bool ShouldDrawForItem(GameObject gameObject)
    {
        string name = gameObject.name;
        return name[0] == '/';
    }
}
