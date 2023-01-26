using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Samples.RebindUI;
using UnityEngine.InputSystem;

public enum InputState
{
    Disabled,
    Gameplay,
    UI
}

public class PersistentInputManager : DescriptionMonoBehavior
{
    [ColorHeader("Listening", ColorHeaderColor.ListeningChannels)]
    [ColorHeader("Input State Change Ask")] 
    [SerializeField] private InputStateEventChannelSO askInputStateChange;
    
    [ColorHeader("Input Dependencies", ColorHeaderColor.Dependencies)]
    [SerializeField] private InputActionAsset inputAsset;
    [SerializeField] private PlayerUserInputProvider inputProvider;

    private const string gameplayMap = "Gameplay";
    private const string uiMap = "UI";

    private InputState currentInputState;

    private void OnEnable() {
        askInputStateChange.OnRaised += SwitchInputState;
    }

    private void OnDisable()
    {
        askInputStateChange.OnRaised -= SwitchInputState;
    }

    private void SwitchInputState(InputState newInputState)
    {
        switch (newInputState)
        {
            case InputState.Disabled:
                DisableAllActionMaps();
                break;
            case InputState.Gameplay:
                SwitchToGameplay();
                break;
            case InputState.UI:
                SwitchToUI();
                break;
        }

        if (currentInputState != newInputState)
        {
            currentInputState = newInputState;
            askInputStateChange.RaiseEvent(newInputState);
        }
    }

    private void SwitchToGameplay()
    {
        SwitchActionMap(gameplayMap);
    }

    public void SwitchToUI()
    {
        SwitchActionMap(uiMap);
    }

    private void SwitchActionMap(string mapName)
    {
        DisableAllActionMaps();
        inputAsset.FindActionMap(mapName).Enable();
    }

    private void DisableAllActionMaps()
    {
        foreach (var map in inputAsset.actionMaps)
            map.Disable();
    }
}

