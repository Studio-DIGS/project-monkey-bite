using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controlling class for drawing hierarchy item overlays
/// </summary>
[InitializeOnLoad]
public class HierarchyItemOverlayDrawer
{
    // Internal class for cache data
    private class ItemOverlayData
    {
        public List<HierarchyItemOverlay> overlays = new List<HierarchyItemOverlay>();
        public List<HierarchyIconSO> iconOverlays = new List<HierarchyIconSO>();
        public bool isDirty = true;
    }
    
    // Cache for overlay data about each Hierarchy gameObject
    private static Dictionary<GameObject, ItemOverlayData> itemDataCache = new Dictionary<GameObject, ItemOverlayData>();

    private static bool initialized;

    private static HierarchyOverlayConfigSO config;

    private static HierarchyItemOverlay separatorOverlay;

    static HierarchyItemOverlayDrawer()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        EditorApplication.hierarchyChanged += MarkCacheAsDirty;
        EditorSceneManager.sceneOpened += SceneOpened;

        SingleActiveConfigSO.AddConfigChangeListener<HierarchyOverlayConfigSO>(ActiveConfigChanged);
    }
    
    // Some hard coded sizing values
    private static float nestingLineTopPadding = 2f;

    /// <summary>
    /// Mark each cache item as dirty
    /// </summary>
    private static void MarkCacheAsDirty()
    {
        foreach (var pair in itemDataCache)
        {
            pair.Value.isDirty = true;
        }
    }

    /// <summary>
    /// Clear cache when a new scene (and thus new hierarchy gameObjects) are opened
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private static void SceneOpened(Scene scene, OpenSceneMode mode)
    {
        itemDataCache.Clear();
    }
    
    /// <summary>
    /// Updates reference to active config and marks cache as dirty. Should be called when changing configs or manual refresh.
    /// Repaints hierarchy for immediate feedback
    /// </summary>
    private static void ActiveConfigChanged()
    {
        config = SingleActiveConfigSO.GetActiveConfig<HierarchyOverlayConfigSO>();
        MarkCacheAsDirty();
        EditorApplication.RepaintHierarchyWindow();
    }

    /// <summary>
    /// Initialize, should only run once per script reload on the first draw frame
    /// </summary>
    private static void Initialize()
    {
        separatorOverlay = new HierarchySeparatorOverlay();
        ActiveConfigChanged();
        initialized = true;
    }

    /// <summary>
    /// Main draw function
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="pos"></param>
    private static void HierarchyWindowItemOnGUI(int instanceID, Rect pos)
    {
        // Initialize during the first draw call, since individual overlays may need access to IMGUI methods
        if (!initialized)
        {
            Initialize();
        }
        
        var gameObject = (GameObject)EditorUtility.InstanceIDToObject(instanceID);

        if (gameObject != null && config != null)
        {
            // Create cache entry if non exists
            if (!itemDataCache.ContainsKey(gameObject))
            {
                itemDataCache.Add(gameObject, new ItemOverlayData());
            }
            
            // Rebuild overlay cache for gameObject if dirty
            if (itemDataCache[gameObject].isDirty)
            {
                BuildObjectOverlayCache(gameObject);
            }
            
            DrawOverlays(gameObject, pos);
        }
    }

    /// <summary>
    /// Draw overlays for a hierarchy item
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="pos"></param>
    private static void DrawOverlays(GameObject gameObject, Rect pos)
    {
        if(config.DrawAlternatingLines)
            DrawAlternatingLineOverlay(pos);
            
        if(config.DrawNestingLines)
            DrawNestingLine(gameObject, pos);

        // Draw overlays present in object cache
        foreach (var overlay in itemDataCache[gameObject].overlays)
        {
            overlay.DrawHierarchyItem(gameObject,pos, config);
        }
            
        if(config.DrawIcons)
            DrawIconOverlays(itemDataCache[gameObject].iconOverlays, pos);
    }

    /// <summary>
    /// Build overlay data for a single cache item
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="selectionRect"></param>
    private static void BuildObjectOverlayCache(GameObject gameObject)
    {
        itemDataCache[gameObject].isDirty = false;

        // Clear cached data
        itemDataCache[gameObject].overlays.Clear();
        itemDataCache[gameObject].iconOverlays.Clear();
        
        // Add separator overlay if valid
        if (separatorOverlay.ShouldDrawForItem(gameObject))
        {
            itemDataCache[gameObject].overlays.Add(separatorOverlay);
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
            bool matchFound = false;
            
            foreach (var iconOverlayGroupConfig in config.IconOverlays)
            {
                
                if (!iconOverlayGroupConfig.isActive || iconOverlayGroupConfig.group == null) continue;

                var iconOverlayGroup = iconOverlayGroupConfig.group;

                // Match the string targets

                string className = componentType.Name.ToLower();

                if (iconOverlayGroup.StringTargetDict.ContainsKey(className))
                {
                    itemDataCache[gameObject].iconOverlays.Add(iconOverlayGroup.StringTargetDict[className]);
                    matchFound = true;
                    break;
                }

                // Match the monoscript targets

                foreach (var monoScriptIcon in iconOverlayGroup.MonoScriptTargetIcons)
                {
                    classReferenceType = monoScriptIcon.targetClass.GetClass();

                    if (!classReferenceType.IsClass) continue;
                
                    // Check if the component is assignable to or a subclass of the icon target class
                    if (classReferenceType.IsAssignableFrom(componentType) || componentType.IsSubclassOf(classReferenceType))
                    {
                        itemDataCache[gameObject].iconOverlays.Add(monoScriptIcon);
                        matchFound = true;
                        break;
                    }
                }
                
                // Only one icon drawn per component
                if (matchFound)
                {
                    break;
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
        var parentTransform = gameObject.transform.parent;
        
        if (parentTransform == null)
            return;
        
        int siblingIndex = gameObject.transform.GetSiblingIndex();
        int totalSiblings = gameObject.transform.parent.childCount;

        bool isLastChild = siblingIndex == totalSiblings - 1;
        bool isFirstChild = siblingIndex == 0;
        bool hasChildren = gameObject.transform.childCount > 0;
        
        pos.x -= EditorGUIExtensions.IndentWidth  * 1.5f - 1;
        
        if (isLastChild)
        {
            // Shortened vertical line
            Rect verticalLine = pos;
            verticalLine.width = 1f;
            verticalLine.height = EditorGUIExtensions.HierarchySingleLineHeight / 2f;

            EditorGUI.DrawRect(verticalLine, config.NestingLineColor);
            
            // Horizontal line
            Rect horizontalLine = pos;
            horizontalLine.height = 1f;
            horizontalLine.y += EditorGUIExtensions.HierarchySingleLineHeight / 2f;
            
            // Shortened horizontal line to make space for dropdown triangle
            horizontalLine.width = hasChildren
                ? EditorGUIExtensions.IndentWidth / 2f
                : EditorGUIExtensions.IndentWidth - 2f;

            EditorGUI.DrawRect(horizontalLine, config.NestingLineColor);
        }
        else
        {
            // Non-last children just draw a simple vertical line
            Rect verticalLine = pos;
            verticalLine.width = 1f;

            // Extra spacing from top if first child
            if (isFirstChild)
            {
                verticalLine.height -= nestingLineTopPadding;
                verticalLine.y += nestingLineTopPadding;
            }

            EditorGUI.DrawRect(verticalLine, config.NestingLineColor);
        }
    }

    private static void DrawIconOverlays(IList<HierarchyIconSO> iconOverlays, Rect pos)
    {
        pos.width = EditorGUIExtensions.HierarchySingleLineHeight - config.iconShrinkAmount;
        pos.height -= config.iconShrinkAmount;
        pos.y += config.iconShrinkAmount / 2f;
        
        // Draw starting from right side of window
        pos.x = EditorGUIUtility.currentViewWidth - pos.width - EditorGUIExtensions.IndentWidth;
        
        // Draw in reverse, so reads left-right in order of components in the gameObject
        for(int i = iconOverlays.Count - 1;i >= 0;i--)
        {
            var iconOverlay = iconOverlays[i];
            GUI.DrawTexture(pos, iconOverlay.icon);
            pos.x -= pos.width;
        }
    }
}