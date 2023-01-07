using UnityEngine;

/// <summary>
/// Base class for drawing a type of item overlay in the hierarchy
/// </summary>
public abstract class HierarchyItemOverlay
{
    public abstract void DrawHierarchyItem(GameObject gameObject, Rect selectionRect, HierarchyOverlayConfigSO config);
    
    public abstract bool ShouldDrawForItem(GameObject gameObject);
}
