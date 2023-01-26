using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Contains the color header color settings. Only one should be active at a time 
/// </summary>
[CreateAssetMenu(menuName = "MushiStuff/MushiEditorCore/Attributes/ColorHeaderConfig")]
public class ColorHeaderConfigSO : SingleActiveConfigSO
{
    public Color[] headerColors;
    
    public Color GetHeaderColor(ColorHeaderColor color)
    {
        return headerColors[(int)color];
    }

    public static ColorHeaderConfigSO GetActiveConfig()
    {
        return SingleActiveConfigSO.GetActiveConfig<ColorHeaderConfigSO>();
    }
}

[CustomEditor(typeof(ColorHeaderConfigSO))]
public class SCHeaderConfigSOInspector : UnityEditor.Editor
{
    private ColorHeaderDrawer dummyDrawer;
    
    public override void OnInspectorGUI()
    {
        dummyDrawer ??= new ColorHeaderDrawer();
        var colorNames = Enum.GetNames(typeof(ColorHeaderColor));
        int newColorCount = colorNames.Length;
        var targetConfig = (ColorHeaderConfigSO) target;
        
        Undo.RecordObject(targetConfig, "Change SCHeader config");
        
        // Active config field
        EditorGUILayout.Space();
        SingleActiveConfigSO.DrawIsActiveField(targetConfig);
        EditorGUILayout.Space();
        
        // Draw color fields for each enum color field 
        int currentColorCount = targetConfig.headerColors.Length;
        
        // If mismatch due to enum updates, then update color field array
        if (newColorCount != currentColorCount)
        {
            UpdateColorFields(targetConfig, newColorCount, currentColorCount);
        }
        
        // Draw color fields for each  element
        for (int i = 0; i < newColorCount; i++)
        {
            Color curr = targetConfig.headerColors[i];
            targetConfig.headerColors[i] = EditorGUILayout.ColorField(colorNames[i], curr);
            if(curr != targetConfig.headerColors[i])
                EditorUtility.SetDirty(targetConfig);
        }
        
        // Draw sample headers
        DrawSampleHeaders(colorNames);
    }

    private void UpdateColorFields(ColorHeaderConfigSO targetSO, int newColorCount, int oldColorCount)
    {
        var newColorFields = new Color[newColorCount];
        
        // Populate with old values
        float populateLength = Mathf.Min(oldColorCount, newColorCount);
        for (int i = 0; i < populateLength; i++)
        {
            newColorFields[i] = targetSO.headerColors[i];
        }

        // Fill in rest with white
        for (int i = oldColorCount; i < newColorCount; i++)
        {
            newColorFields[i] = Color.white;
        }

        // Update field to new list
        targetSO.headerColors = newColorFields;
    }

    private void DrawSampleHeaders(string[] colorNames)
    {
        // Reflection setup
        var style = new GUIStyle(EditorStyles.boldLabel);
        FieldInfo attributeProperty = typeof(DecoratorDrawer).GetField("m_Attribute", BindingFlags.NonPublic | BindingFlags.Instance);
        
        // Title
        style.alignment = TextAnchor.LowerCenter;
        style.normal.textColor = Color.white;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Header Samples", style);
        // Draw sample header for each color, with two dummy fields for visual context
        float width = EditorGUIUtility.fieldWidth;
        for (int i = 0; i < colorNames.Length; i++)
        {
            string colorName = colorNames[i];
            var dummyAttribute = new ColorHeaderAttribute(colorName, (ColorHeaderColor)i);

            // Cursed reflection to set the attribute of the attribute drawer
            attributeProperty.SetValue(dummyDrawer, dummyAttribute);
            
            dummyDrawer.SetConfig((ColorHeaderConfigSO)target);
            
            // Draw Header
            float headerHeight = dummyDrawer.GetHeight();
            Rect headerRect = GUILayoutUtility.GetRect(width, headerHeight);
            dummyDrawer.OnGUI(headerRect);

            // Draw dummy 
            EditorGUILayout.ObjectField("Dummy Field", null, typeof(UnityEngine.Object), false);
            EditorGUILayout.FloatField("Dummy Field", 0f);
        }
    }
}
