using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Samples.RebindUI;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Tooltip("Reference to Play Input component.")]
    [SerializeField]
    private PlayerInput playerInputComponent;
    
    [Tooltip("Reference to Used Action Asset.")]
    [SerializeField]
    private InputActionAsset asset;

    [SerializeField] private GameplayMapInputProvider provider;
    
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
        provider.SetupEvents();
        provider.OnPausePressed += SwitchToUI;
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
        asset.FindActionMap(uiMap).Disable();
        playerInputComponent.SwitchCurrentActionMap(name);
    }
}
