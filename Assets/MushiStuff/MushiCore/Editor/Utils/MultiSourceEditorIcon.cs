using System;
using UnityEditor;
using UnityEngine;

namespace MushiCore.Editor
{
    /// <summary>
    /// Simple editor class representing gui content with a texture from multiple possible sources
    /// Intended for use in tool config SOs
    /// </summary>
    [Serializable]
    public class MultiSourceEditorIcon
    {
        public enum IconType
        {
            EditorGUIUtility,
            TextureAsset
        }

        // Public fields
        // Icon fields
        public IconType iconType;
        public string editorIconStringIdentifier;
        public Texture2D textureAsset;

        public GUIContent Content => GetContent();

        private GUIContent GetContent()
        {
            if (iconType == IconType.EditorGUIUtility)
            {
                return EditorGUIUtility.IconContent(editorIconStringIdentifier);
            }
            else
            {
                return new GUIContent(textureAsset);
            }
        }

        public static implicit operator GUIContent(MultiSourceEditorIcon icon)
        {
            return icon.GetContent();
        }

        public static implicit operator Texture(MultiSourceEditorIcon icon)
        {
            if (icon.iconType == IconType.EditorGUIUtility)
            {
                return icon.Content.image;
            }
            else
            {
                return icon.textureAsset;
            }
        }
    }

    [CustomPropertyDrawer(typeof(MultiSourceEditorIcon))]
    public class MultiSourceEditorIconDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var iconTypeProp = property.FindPropertyRelative("iconType");
            position.height = EditorGUIUtility.singleLineHeight;
            // Icon Type dropdown
            EditorGUI.PropertyField(position, iconTypeProp);

            // Set up positions for icon data fields
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // Built in icon string field
            if (iconTypeProp.enumValueIndex == (int)MultiSourceEditorIcon.IconType.EditorGUIUtility)
            {
                var iconStringProp = property.FindPropertyRelative("editorIconStringIdentifier");
                EditorGUI.PropertyField(position, iconStringProp);
            }
            // Custom texture asset field
            else if (iconTypeProp.enumValueIndex == (int)MultiSourceEditorIcon.IconType.TextureAsset)
            {
                var iconTextureProp = property.FindPropertyRelative("textureAsset");
                EditorGUI.PropertyField(position, iconTextureProp);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}