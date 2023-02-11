using MushiCore.Editor;
using MushiCore.EditorAttributes;
using UnityEngine;

namespace MushiEditorTools.HierarchyOverlay
{
    public class HierarchyIconSO : ScriptableObject
    {
        [ColorHeader("Icon")]
        public MultiSourceEditorIcon icon;
    }
}