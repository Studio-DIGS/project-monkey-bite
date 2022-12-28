using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [ColorHeader("Listening - On Main Menu Scene Loaded Channel", ColorHeaderColor.ListeningEvents)]
    [SerializeField] private VoidEventChannelSO onMainMenuSceneLoaded;

    [ColorHeader("Invoking - Ask Change Input State Channel", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private InputStateEventChannelSO askInputStateChange;

    [ColorHeader("Listening - Change Main Menu Page Ask Channel")] 
    [SerializeField] private MenuPageEventChannelSO askChangeMenuPage;
    
    [ColorHeader("Invoking - Ask Change Game State Channel")] 
    [SerializeField] private GameStateEventChannelSO askChangeGameState;
    
    [ColorHeader("Listening - Enter Save Profile Ask Event")] 
    [SerializeField] private ProfileSaveDataEventChannelSO askEnterSaveProfile;

    [ColorHeader("Initial Selection", ColorHeaderColor.Config)] 
    [SerializeField] private MenuPage initialActiveMenuPage;

    [SerializeField] private ProfileSaveDataSO activeSaveContainer;
    
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

    private void EnterSaveProfile(ProfileSaveData data)
    {
        activeSaveContainer.profileSaveData = data;
        askChangeGameState.RaiseEvent(GameState.Gameplay);
    }
}
