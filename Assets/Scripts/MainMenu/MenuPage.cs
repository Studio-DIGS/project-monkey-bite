using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : DescriptionMonoBehavior
{
    [ColorHeader("MenuPage UI Dependencies", ColorHeaderColor.Dependencies)]
    [SerializeField] protected CanvasGroup pageCanvasGroup;
    
    [ColorHeader("Invoking - Ask Display Menu Page Event", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] protected MenuPageEventChannelSO askChangeMenuPage;
    
    protected virtual void OnEnable()
    {
        pageCanvasGroup.SetVisible(false);
    }

    public virtual void ShowPage()
    {
        pageCanvasGroup.SetVisible(true);
    }

    public virtual void HidePage()
    {
        pageCanvasGroup.SetVisible(false);
    }
}
