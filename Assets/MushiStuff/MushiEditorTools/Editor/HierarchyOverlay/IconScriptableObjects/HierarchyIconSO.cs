using System.Collections.Generic;
using MushiCore.Editor;
using UnityEditor;
using UnityEngine;

namespace MushiEditorTools.HierarchyOverlay
{
    [CreateAssetMenu(menuName = "MushiStuff/MushiEditorTools/Hierarchy Overlays/HierarchyIconSO")]
    public class HierarchyIconSO : ScriptableObject
    {
        [SerializeField, TextArea] private string description;

        [ColorHeader("Icon")]
        public MultiSourceEditorIcon icon;

        [ColorHeader("Targets")]
        public List<MonoScript> targetClassMonoscripts;

        public List<string> targetClassStrings;
    }
}