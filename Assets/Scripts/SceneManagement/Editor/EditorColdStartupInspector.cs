using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EditorColdStartup))]
public class EditorColdStartupInspector : Editor
{
    private SerializedObject so;

    private void OnEnable()
    {
        var component = (EditorColdStartup)target;
        so = new SerializedObject(component);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        bool isIsolated = ((EditorColdStartup)target).IsIsolated();

        void CreatePropField(string propName)
        {
            var prop = so.FindProperty(propName);
            EditorGUILayout.PropertyField(prop, true);
        }
        
        if (isIsolated)
        {
            CreatePropField("startupInputState");
            CreatePropField("askInputStateChange");
            CreatePropField("isolatedManualRaiseChannels");
        }

        so.ApplyModifiedProperties();
    }
}
