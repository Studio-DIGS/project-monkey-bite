using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EditorColdStartup))]
public class EditorColdStartupInspector : Editor
{
    public override void OnInspectorGUI()
    {
        var component = (EditorColdStartup)target;
        base.OnInspectorGUI();

        var so = new SerializedObject(component);
        var isIsolated = so.FindProperty("thisSceneData");
        var isContent = so.FindProperty("isContentScene");
        
        void CreatePropField(string propName)
        {
            var prop = so.FindProperty(propName);
            EditorGUILayout.PropertyField(prop);
        }
        
        if (!isContent.boolValue)
        {
            CreatePropField("managerSceneReadyChannel");
        }
        else
        {
            if (isIsolated.objectReferenceValue == null)
            {
                CreatePropField("contentSceneReadyChannel");
                CreatePropField("setInputStateChannel");
                CreatePropField("startupInputState");
            }
        }

        so.ApplyModifiedProperties();
    }
}
