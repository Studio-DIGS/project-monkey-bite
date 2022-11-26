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

public class InputManager : MonoBehaviour
{
    [Header("Listening - Request Input State Change Channels")] 
    [SerializeField] private RequestInputStateChangeEventChannelSO inputStateChange;
    
    [Header("Input Assets/Dependencies")]
    [Tooltip("Reference to Used Action Asset.")] 
    [SerializeField] private InputActionAsset asset;
    [SerializeField] private PlayerUserInputProvider provider;

    private const string gameplayMap = "Gameplay";
    private const string uiMap = "UI";

    private InputState currentInputState;

    private void OnEnable() {
        inputStateChange.OnRequestInputStateChange += SwitchInputState;
    }

    private void OnDisable()
    {
        inputStateChange.OnRequestInputStateChange -= SwitchInputState;
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
            inputStateChange.RaiseEvent(newInputState);
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
        asset.FindActionMap(mapName).Enable();
    }

    private void DisableAllActionMaps()
    {
        foreach (var map in asset.actionMaps)
            map.Disable();
    }
}

