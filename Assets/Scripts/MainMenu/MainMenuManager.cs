using System;
using System.Collections;
using System.Collections.Generic;
using MushiCore.EditorAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : DescriptionMonoBehavior
{
    [ColorHeader("Listening", ColorHeaderColor.ListeningChannels)]
    [ColorHeader("On Main Menu Scene Loaded")]
    [SerializeField] private VoidEventChannelSO onMainMenuSceneLoaded;
    
    [ColorHeader("Change Main Menu Page Ask")] 
    [SerializeField] private MenuPageEventChannelSO askChangeMenuPage;
    
    [ColorHeader("Enter Save Profile Ask")] 
    [SerializeField] private SaveProfileDataEventChannelSO askEnterSaveProfile;

    [ColorHeader("Invoking", ColorHeaderColor.InvokingChannels)]
    [ColorHeader("Ask Change Input State")]
    [SerializeField] private InputStateEventChannelSO askInputStateChange;

    [ColorHeader("Ask Change Game State")] 
    [SerializeField] private GameStateEventChannelSO askChangeGameState;
    
    [ColorHeader("Ask Set Active Save Profile")] 
    [SerializeField] private SaveProfileDataEventChannelSO askSetActiveSaveProfile;

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
