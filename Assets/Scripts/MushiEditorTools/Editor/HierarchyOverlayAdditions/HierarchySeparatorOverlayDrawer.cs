using System;
using UnityEditor;
using UnityEditor.SceneManagement;
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
        if (!config.drawSeparators) return;
        
        selectionRect.x = 0; 
        selectionRect.width = EditorGUIUtility.currentViewWidth;
        
        // Background
        EditorGUI.DrawRect(selectionRect, config.separatorBackgroundColor);
                
        // Icon
        iconContent = config.separatorIcon;
        
        // Text style
        overlayStyle.normal.textColor = config.separatorTextColor;
        overlayStyle.hover.textColor = config.separatorTextColor;
        
        // Setup text
        string displayName = gameObject.name.Trim('/');
        iconContent.text = displayName;

        EditorGUI.LabelField(selectionRect, iconContent, overlayStyle);
    }

    public override bool ShouldDrawForItem(GameObject gameObject)
    {
        string name = gameObject.name;
        return name[0] == '/';
    }
}
