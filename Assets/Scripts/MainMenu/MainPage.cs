using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainPage : MenuPage
{
    [SerializeField] private GameObject initialSelected;
    [SerializeField] private Button playButton;
    [SerializeField] private MenuPage onPlayButtonPage;
    
    public override void ShowPage()
    {
        base.ShowPage();
        EventSystem.current.SetSelectedGameObject(initialSelected);
        playButton.onClick.AddListener(OnPlayButton);
    }

    public override void HidePage()
    {
        base.HidePage();
        playButton.onClick.RemoveListener(OnPlayButton);
    }

    private void OnPlayButton()
    {
        askChangeMenuPage.RaiseEvent(onPlayButtonPage);
    }
}
