using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : DescriptionMonoBehavior
{
    [ColorHeader("Listening - On Main Menu Scene Loaded Channel", ColorHeaderColor.ListeningEvents)]
    [SerializeField] private VoidEventChannelSO onMainMenuSceneLoaded;

    [ColorHeader("Invoking - Ask Change Input State Channel", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private InputStateEventChannelSO askInputStateChange;

    [ColorHeader("Listening - Change Main Menu Page Ask Channel")] 
    [SerializeField] private MenuPageEventChannelSO askChangeMenuPage;
    
    [ColorHeader("Invoking - Ask Change Game State Channel")] 
    [SerializeField] private GameStateEventChannelSO askChangeGameState;
    
    [ColorHeader("Invoking - Ask Set Active Profile Save Channel")] 
    [SerializeField] private SaveProfileDataEventChannelSO askSetActiveSaveProfile;
    
    [ColorHeader("Listening - Enter Save Profile Ask Event")] 
    [SerializeField] private SaveProfileDataEventChannelSO askEnterSaveProfile;

    [ColorHeader("Initial Selection", ColorHeaderColor.Config)] 
    [SerializeField] private MenuPage initialActiveMenuPage;

#if UNITY_EDITOR
    [ColorHeader("Reading - Cold Startup State", ColorHeaderColor.ReadingState)]
    [SerializeField] private ColdStartupDataSO coldStartupState;
#endif

    private MenuPage currentShownMenuPage;
    
    void OnEnable()
    {
        onMainMenuSceneLoaded.OnRaised += SetupMainMenu;
        askChangeMenuPage.OnRaised += ShowPage;
        askEnterSaveProfile.OnRaised += EnterSaveProfile;
    }

    private void OnDisable()
    {
        onMainMenuSceneLoaded.OnRaised -= SetupMainMenu;
        askChangeMenuPage.OnRaised -= ShowPage;
        askEnterSaveProfile.OnRaised -= EnterSaveProfile;
    }

    private void SetupMainMenu()
    {
        
#if UNITY_EDITOR
            if (coldStartupState.isColdStartup)
        {
            coldStartupState.ConsumeColdStartup();
        }
#endif
        
        askInputStateChange.RaiseEvent(InputState.UI);
        ShowPage(initialActiveMenuPage);
    }

    private void ShowPage(MenuPage menuPage)
    {
        if (currentShownMenuPage)
        {
            currentShownMenuPage.HidePage();
        }
        menuPage.ShowPage();
        currentShownMenuPage = menuPage;
    }

    private void EnterSaveProfile(SaveProfileData data)
    {
        askSetActiveSaveProfile.RaiseEvent(data);
        askChangeGameState.RaiseEvent(GameState.Gameplay);
    }
}
