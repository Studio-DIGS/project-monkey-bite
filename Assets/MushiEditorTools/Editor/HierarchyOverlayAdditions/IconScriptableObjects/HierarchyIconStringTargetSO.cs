using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "MushiTools/Hierarchy Overlays/Icons/HierarchyIconStringTarget")]
public class HierarchyIconStringTargetSO : HierarchyIconSO
{
    [ColorHeader("Target")]
    public string targetClassString;
}
