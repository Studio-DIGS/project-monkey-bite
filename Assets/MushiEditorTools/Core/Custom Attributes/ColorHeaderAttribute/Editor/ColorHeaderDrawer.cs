using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ColorHeaderAttribute))]
public class ColorHeaderDrawer : DecoratorDrawer
{
    private static ColorHeaderConfigSO activeConfig;
    private GUIStyle headerStyle;
    private bool initialized;
    

    public override void OnGUI(Rect position)
    {
        if (!initialized)
        {
            Initialize();
        }

        ColorHeaderAttribute attr = (ColorHeaderAttribute)attribute;

        DrawHeader(position, attr);
    }

    private void Initialize()
    {
        initialized = true;
        activeConfig = ColorHeaderConfigSO.GetActiveConfig();
        CreateHeaderStyle();
    }

    public void SetConfig(ColorHeaderConfigSO configConfig)
    {
        activeConfig = configConfig;
        CreateHeaderStyle();
    }

    private void CreateHeaderStyle()
    {
        ColorHeaderAttribute attr = (ColorHeaderAttribute)attribute;
        
        headerStyle = new GUIStyle(EditorStyles.boldLabel);
        
        var textColor = Color.white;
        if (activeConfig != null)
        {
            textColor = activeConfig.GetHeaderColor(attr.color);
        }

        headerStyle.normal.textColor = textColor;
        headerStyle.hover.textColor = textColor * 0.95f;
    }

    private void DrawHeader(Rect position, ColorHeaderAttribute attr)
    {
        // Y Offset
        position.yMin += EditorGUIUtility.singleLineHeight * attr.yOffset;
        // Indent offset
        position = EditorGUI.IndentedRect(position);
        // Draw header
        GUI.Label(position, attr.text, headerStyle);
    }

    public override float GetHeight()
    {
        ColorHeaderAttribute attr = (ColorHeaderAttribute)attribute;
        float fullTextHeight = EditorStyles.boldLabel.CalcHeight(new GUIContent(attr.text), 1.0f);
        return EditorGUIUtility.singleLineHeight * attr.padding * 2 + fullTextHeight;
    }
}
