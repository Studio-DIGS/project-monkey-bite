using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class HierarchyIconOverlay
{
    public enum TargetFormat
    {
        MonoScript,
        TypeStringMatch
    }

    public TargetFormat targetFormat;
    public MultiSourceEditorIcon icon;
    public string targetClassString;
    public MonoScript targetClass;
}
