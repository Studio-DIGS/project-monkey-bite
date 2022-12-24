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

    [ColorHeader("Invoking - Main Menu Setup Channels", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private InputStateEventChannelSO askInputStateChange;
    
    [ColorHeader("Invoking - Ask Game State Change", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private GameStateEventChannelSO askGameStateChange;


    [ColorHeader("Initial Selection", ColorHeaderColor.Config)] 
    [SerializeField] private GameObject initialSelectedGameObject;
    
#if UNITY_EDITOR
    [ColorHeader("Reading - Cold Startup State", ColorHeaderColor.ReadingState)]
    [SerializeField] private ColdStartupDataSO coldStartupState;
#endif
    
    void OnEnable()
    {
        onMainMenuSceneLoaded.OnRaised += SetupMainMenu;
    }

    private void OnDisable()
    {
        onMainMenuSceneLoaded.OnRaised -= SetupMainMenu;
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
        EventSystem.current.SetSelectedGameObject(initialSelectedGameObject);
    }

    public void OnPlayButton()
    {
        askInputStateChange.RaiseEvent(InputState.Disabled);
        askGameStateChange.RaiseEvent(GameState.Gameplay);
    }
}
