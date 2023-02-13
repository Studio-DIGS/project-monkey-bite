using MushiCore.EditorAttributes;
using UnityEditor;
using UnityEngine;

namespace MushiEditorTools.HierarchyOverlay
{
    [CreateAssetMenu(menuName = "MushiStuff/MushiEditorTools/Hierarchy Overlays/Icons/HierarchyIconMonoScriptTarget")]
    public class HierarchyIconMonoScriptTargetSO : HierarchyIconSO
    {
        [ColorHeader("Target")]
        public MonoScript targetClass;
    }
}