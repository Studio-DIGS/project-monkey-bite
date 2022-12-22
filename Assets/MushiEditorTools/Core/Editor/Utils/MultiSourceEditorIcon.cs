using System;
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
    private enum IconType
    {
        EditorGUIUtility,
        TextureAsset
    }

    [SerializeField] private IconType iconType;
    [SerializeField] private string editorIconStringIdentifier;
    [SerializeField] private Texture2D textureAsset;
    
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
            guiContent = EditorGUIUtility.IconContent(editorIconStringIdentifier);
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
        if (icon.iconType == IconType.EditorGUIUtility)
        {
            return icon.GUIContent.image;
        }
        else
        {
            return icon.textureAsset;
        }
    }
}

/*[CustomPropertyDrawer(typeof(MultiSourceEditorIcon))]
public class MultiSourceEditorIconDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Undo.RecordObject(target, "MultiSourceEditorIcon change");
        target.iconType = (MultiSourceEditorIcon.IconType)EditorGUI.EnumPopup(position, label, target.iconType);

        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        
        if (target.iconType == MultiSourceEditorIcon.IconType.EditorGUIUtility)
        {
            target.editorIconStringIdentifier = EditorGUI.TextField(position, target.editorIconStringIdentifier);
        }
        else if (target.iconType == MultiSourceEditorIcon.IconType.TextureAsset)
        {
            target.textureAsset = (Texture2D)EditorGUI.ObjectField(position, target.textureAsset, typeof(Texture2D));
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
    }
}*/
