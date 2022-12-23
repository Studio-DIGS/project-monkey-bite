using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinishLocation : MonoBehaviour
{
    [ColorHeader("Invoking - On Level Completed", ColorHeaderColor.TriggeringEvents)]
    [SerializeField] private VoidEventChannelSO onLevelCompleted;

    private bool finished = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!finished && other.GetComponentInChildren<TestCharacter>() != null)
        {
            FinishLevel();
            finished = true;
        }
    }

    [ContextMenu("Debug - Finish Level")]
    private void FinishLevel()
    {
        onLevelCompleted.RaiseEvent();
    }
}
