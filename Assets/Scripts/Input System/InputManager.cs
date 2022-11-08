using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Samples.RebindUI;
using UnityEngine.InputSystem;

// TODO When the game state manager gets added, move all of the input state change logic out - the game state manager should handle changing input states

public class InputManager : MonoBehaviour
{
    [Tooltip("Reference to Play Input component.")] [SerializeField]
    private PlayerInput playerInputComponent;

    [Tooltip("Reference to Used Action Asset.")] [SerializeField]
    private InputActionAsset asset;

    [SerializeField] private PlayerUserInputProvider provider;

    enum InputState
    {
        Gameplay,
        UI
    }

    private const string gameplayMap = "Gameplay";
    private const string uiMap = "UI";

    private void Start()
    {
        SwitchToGameplay();
        provider.Events.OnPausePressed += SwitchToUI;
    }

    public void SwitchToGameplay()
    {
        SwitchActionMap(gameplayMap);
    }

    public void SwitchToUI()
    {
        SwitchActionMap(uiMap);
    }

    private void SwitchActionMap(string name)
    {
        foreach (var map in asset.actionMaps)
            map.Disable();
        asset.FindActionMap(name).Enable();
        playerInputComponent.SwitchCurrentActionMap(name);
    }
}

