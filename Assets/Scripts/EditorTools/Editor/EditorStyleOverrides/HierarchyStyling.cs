using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[InitializeOnLoad]
public static class HierarchyStyling
{
    static readonly GUIStyle style;

    static HierarchyStyling()
    {
        style = new GUIStyle();
        // Style
        style.richText = true;
        style.alignment = TextAnchor.MiddleCenter;
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
    }

    static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (gameObject != null && gameObject.name.StartsWith("=", StringComparison.Ordinal))
        {
            Rect backgroundRect = selectionRect;
            EditorGUI.DrawRect(backgroundRect, new Color(0.15f, 0.15f, 0.15f));
            EditorGUI.LabelField(selectionRect, $"<color=#97CFCE>{gameObject.name.Replace("=", "")}</color>", style);
        }
    }
}