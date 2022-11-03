using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPage : MonoBehaviour
{
    [SerializeField] private List<SettingsGroup> containedGroups;

    [Header("UI Components")] 
    
    [SerializeField] private CanvasGroup pageCanvasGroup;

    public void Awake()
    {
        foreach (SettingsGroup group in containedGroups)
        {
            group.LoadSavedSettings();
        }
    }

    public void OpenPage()
    {
        pageCanvasGroup.SetVisible(true);
        foreach (SettingsGroup group in containedGroups)
        {
            group.BeginSession();
        }
    }

    public void Save()
    {
        foreach (SettingsGroup group in containedGroups)
        {
            group.SaveChanges();
        }
    }

    public void ClosePage()
    {
        pageCanvasGroup.SetVisible(false);
        foreach (SettingsGroup group in containedGroups)
        {
            group.CloseSession();
        }
    }
}
