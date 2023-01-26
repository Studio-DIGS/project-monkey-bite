using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "MushiStuff/MushiEditorTools/Hierarchy Overlays/Icons/HierarchyIconMonoScriptTarget")]
public class HierarchyIconMonoScriptTargetSO : HierarchyIconSO
{
    [ColorHeader("Target")]
    public MonoScript targetClass;
}
