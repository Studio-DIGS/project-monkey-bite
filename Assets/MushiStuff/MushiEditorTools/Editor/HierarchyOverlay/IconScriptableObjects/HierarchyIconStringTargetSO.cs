using MushiCore.EditorAttributes;
using UnityEngine;

namespace MushiEditorTools.HierarchyOverlay
{
    [CreateAssetMenu(menuName = "MushiStuff/MushiEditorTools/Hierarchy Overlays/Icons/HierarchyIconStringTarget")]
    public class HierarchyIconStringTargetSO : HierarchyIconSO
    {
        [ColorHeader("Target")]
        public string targetClassString;
    }
}