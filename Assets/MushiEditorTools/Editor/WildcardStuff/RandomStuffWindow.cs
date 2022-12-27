using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class RandomStuffWindow : EditorWindow
{
    [MenuItem("MushiTools/RandomStuffWindow")]
    public static void ShowWindow()
    {
        RandomStuffWindow wnd = GetWindow<RandomStuffWindow>("Random Stuff");
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Open Persistent Data Folder"))
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }
}
