using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RunFinishMenuManager : MonoBehaviour
{
    [ColorHeader("Reading - Gameplay Run State", ColorHeaderColor.ReadingState)] 
    [SerializeField] private GameplayRunStateSO currentRunState;

    [ColorHeader("Listening - On Menu Scene Ready", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private VoidEventChannelSO onRunFinishSceneReady;
    
    [ColorHeader("Invoking - Ask Input State Change", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private InputStateEventChannelSO askInputStateChange;

    [ColorHeader("Invoking - On Continue Button Pressed", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private VoidEventChannelSO onContinueButtonPressed;
    
    [ColorHeader("Invoking - On Main Menu Button Pressed", ColorHeaderColor.ListeningEvents)] 
    [SerializeField] private VoidEventChannelSO onMainMenuButtonPressed;
    
    [ColorHeader("UI Components", ColorHeaderColor.Dependencies)] 
    [SerializeField] private Button continueButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI successText;
    
    private void OnEnable()
    {
        onRunFinishSceneReady.OnRaised += FinishRun;
        
        continueButton.onClick.AddListener(Continue);
        mainMenuButton.onClick.AddListener(MainMenu);
    }

    private void OnDisable()
    {
        onRunFinishSceneReady.OnRaised -= FinishRun;
        
        continueButton.onClick.RemoveListener(Continue);
        mainMenuButton.onClick.RemoveListener(MainMenu);
    }

    private void FinishRun()
    {
        DisplayRunFinishUI();
        // Link up continue button when all the run end operations are finished
        // Should do saving operations here

        askInputStateChange.RaiseEvent(InputState.UI);
        EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
    }

    private void DisplayRunFinishUI()
    {
        var completionState = currentRunState.RunCompletionState;
        if (completionState.playerDied)
        {
            successText.text = "You ded";
        }
        else if(!completionState.runRequirementsMet)
        {
            successText.text = "Objectives Failed";
        }
        else
        {
            successText.text = "U win :D";
        }
    }

    private void Continue()
    {
        onContinueButtonPressed.RaiseEvent();
    }

    private void MainMenu()
    {
        onMainMenuButtonPressed.RaiseEvent();
    }
}
