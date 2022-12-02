using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [Header("Listening")]
    [SerializeField] private VoidEventChannelSO mainMenuSceneReady;

    [Header("Invoking")]
    [SerializeField] private RequestInputStateChangeEventChannelSO requestInputStateChange;

    [Header("Dependencies")] 
    [SerializeField] private GameObject initialSelectedGameObject;
    
    void OnEnable()
    {
        mainMenuSceneReady.OnEventRaised += SetupMainMenu;
    }

    private void OnDisable()
    {
        mainMenuSceneReady.OnEventRaised -= SetupMainMenu;
    }

    private void SetupMainMenu()
    {
        requestInputStateChange.RaiseEvent(InputState.UI);
        EventSystem.current.SetSelectedGameObject(initialSelectedGameObject);
    }
}
