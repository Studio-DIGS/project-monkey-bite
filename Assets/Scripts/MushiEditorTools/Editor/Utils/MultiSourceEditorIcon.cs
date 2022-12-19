using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Simple editor class representing gui content with a texture from multiple possible sources
/// Intended for use in tool config SOs
/// </summary>
[System.Serializable]
public class MultiSourceEditorIcon
{
    public enum IconType
    {
        EditorGUIUtility,
        TextureAsset
    }

    public IconType iconType;
    public string editorIconStringIdentifier;
    public Texture2D textureAsset;

    private GUIContent guiContent;
    public GUIContent GUIContent
    {
        get
        {
            UpdateContent();
            return guiContent;
        }
    }

    private void UpdateContent()
    {
        if (iconType == IconType.EditorGUIUtility)
        {
            guiContent = new GUIContent(EditorGUIUtility.IconContent(editorIconStringIdentifier));
        }
        else
        {
            guiContent = new GUIContent(textureAsset);
        }
    }
    
    public static implicit operator GUIContent(MultiSourceEditorIcon icon)
    {
        return icon.GUIContent;
    }
    
    public static implicit operator Texture(MultiSourceEditorIcon icon)
    {
        return icon.GUIContent.image;
    }
}

/*
[CustomPropertyDrawer(typeof(MultiSourceEditorIcon))]
public class MultiSourceEditorIconDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUILayout.def
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;// * 2 + EditorGUIUtility.standardVerticalSpacing;
    }
}*/
