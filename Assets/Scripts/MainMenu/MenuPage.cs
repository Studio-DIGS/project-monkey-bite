using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : DescriptionMonoBehavior
{
    [SerializeField] protected CanvasGroup pageCanvasGroup;
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
