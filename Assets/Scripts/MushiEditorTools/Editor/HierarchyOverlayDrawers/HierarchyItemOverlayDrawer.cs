using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Controlling class for drawing hierarchy item overlays
/// </summary>
[InitializeOnLoad]
public class HierarchyItemOverlayDrawer
{
    private static List<HierarchyItemOverlay> itemOverlays;
    private static Dictionary<GameObject, ItemOverlayData> itemDataCache = new Dictionary<GameObject, ItemOverlayData>();

    private static bool initialized;
    private static HierarchyOverlayConfigSO config;

    private static bool isCacheDirty = true;
    
    static HierarchyItemOverlayDrawer()
    {
        itemOverlays = new List<HierarchyItemOverlay>();

        UpdateConfig();
        
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        EditorApplication.hierarchyChanged += HierarchyWindowChanged;
        
        SingleActiveConfigSO.AddConfigChangeListener<HierarchyOverlayConfigSO>(UpdateConfig);
    }

    private static void CreateItemOverlays()
    {
        itemOverlays.Clear();
        itemOverlays.Add(new HierarchySeparatorOverlay());
    }

    private static void HierarchyWindowChanged()
    {
        itemDataCache.Clear();
        isCacheDirty = true;
    }

    private static void UpdateConfig()
    {
        config = SingleActiveConfigSO.GetActiveConfig<HierarchyOverlayConfigSO>();
    }

    private static void HierarchyWindowItemOnGUI(int instanceID, Rect pos)
    {
        if (!initialized)
        {
            CreateItemOverlays();
            initialized = true;
        }
        
        var gameObject = (GameObject)EditorUtility.InstanceIDToObject(instanceID);

        if (gameObject != null && config != null)
        {
            CacheObjectOverlayData(gameObject, pos);
            
            DrawAlternatingLineOverlay(pos);
            
            DrawNestingLine(gameObject, pos);

            // Run through list of valid overlays and draw
            foreach (var overlay in itemDataCache[gameObject].overlays)
            {
                overlay.DrawHierarchyItem(gameObject,pos, config);
            }
            
            // Draw icon overlays
            DrawIconOverlays(itemDataCache[gameObject].iconOverlays, pos);
        }
    }

    private static void CacheObjectOverlayData(GameObject gameObject, Rect selectionRect)
    {
        itemDataCache.TryAdd(gameObject, new ItemOverlayData());
        
        // Clear cached data
        itemDataCache[gameObject].overlays.Clear();
        itemDataCache[gameObject].iconOverlays.Clear();
        
        // Store item's position in dictionary
        itemDataCache[gameObject].selectionRect = selectionRect;

        // Stores valid overlays for this item
        foreach (var overlay in itemOverlays)
        {
            if (overlay.ShouldDrawForItem(gameObject))
            {
                itemDataCache[gameObject].overlays.Add(overlay);
            }
        }
        
        // Icon overlays
        var components = gameObject.GetComponents<Component>();
        Type classReferenceType;
        Type componentType;
        
        // Iterate over each component
        foreach (var component in components)
        {
            if (component == null) continue;
            
            componentType = component.GetType();

            // Check if the component is a valid target of some icon overlay
            foreach (var iconOverlay in config.IconOverlays)
            {
                if (iconOverlay == null) continue;

                // Match types from object field
                if (iconOverlay.targetFormat == HierarchyIconOverlay.TargetFormat.MonoScript)
                {
                    classReferenceType = iconOverlay.targetClass.GetClass();

                    if (!classReferenceType.IsClass) continue;
                
                    // Check if the component is assignable to or a subclass of the icon target class
                    if (classReferenceType.IsAssignableFrom(componentType) || componentType.IsSubclassOf(classReferenceType))
                    {
                        itemDataCache[gameObject].iconOverlays.Add(iconOverlay);
                        break;
                    }
                }
                // Match types from string
                else
                {
                    if (componentType.Name.ContainsInsensitive(iconOverlay.targetClassString))
                    {
                        itemDataCache[gameObject].iconOverlays.Add(iconOverlay);
                        break;
                    }
                }
            }
        }
    }

    private static void DrawAlternatingLineOverlay(Rect pos)
    {
        int count = (int)(pos.y / EditorGUIExtensions.HierarchySingleLineHeight);
        if (count % 2 == 0)
        {
            pos.x = 0;
            pos.width = EditorGUIUtility.currentViewWidth;
            EditorGUI.DrawRect(pos, config.AlternatingLineOverlay);
        }
    }
    
    private static void DrawNestingLine(GameObject gameObject, Rect pos)
    {
        if (gameObject.transform.parent == null)
            return;
        
        int siblingIndex = gameObject.transform.GetSiblingIndex();
        int totalSiblings = gameObject.transform.parent.childCount;

        bool isLastChild = siblingIndex == totalSiblings - 1;
        bool hasChildren = gameObject.transform.childCount > 0;
        
        if (isLastChild)
        {
            pos.x -= EditorGUIExtensions.IndentWidth  * 1.5f - 1;

            // Assumes the parent has been drawn if the child is currently being drawn
            var parentGameObject = gameObject.transform.parent.gameObject;
            Rect parentPosition = itemDataCache[parentGameObject].selectionRect;
            
            // Height to reach parent rect
            float yDist = pos.y - parentPosition.y;

            // Vertical line
            Rect verticalLine = pos;
            verticalLine.width = 1f;
            verticalLine.height = yDist - 10f;
            verticalLine.y += 18f - yDist;

            EditorGUI.DrawRect(verticalLine, config.NestingLineColor);
        
            // Horizontal line
            Rect horizontalLine = pos;
            horizontalLine.height = 1f;
            horizontalLine.y += 8f;
            
            horizontalLine.width = hasChildren
                ? EditorGUIExtensions.IndentWidth / 2f
                : EditorGUIExtensions.IndentWidth - 2f;

            EditorGUI.DrawRect(horizontalLine, config.NestingLineColor);
        }
    }

    private static void DrawIconOverlays(IEnumerable<HierarchyIconOverlay> iconOverlays, Rect pos)
    {
        pos.width = EditorGUIExtensions.HierarchySingleLineHeight - config.iconShrinkAmount;
        pos.height -= config.iconShrinkAmount;
        pos.y += config.iconShrinkAmount / 2f;
        pos.x = EditorGUIUtility.currentViewWidth - pos.width - EditorGUIExtensions.IndentWidth;
        foreach (var iconOverlay in iconOverlays)
        {
            GUI.DrawTexture(pos, iconOverlay.icon);
            pos.x -= pos.width;
        }
    }
}

public class ItemOverlayData
{
    public List<HierarchyItemOverlay> overlays = new List<HierarchyItemOverlay>();
    public List<HierarchyIconOverlay> iconOverlays = new List<HierarchyIconOverlay>();
    public Rect selectionRect;
}