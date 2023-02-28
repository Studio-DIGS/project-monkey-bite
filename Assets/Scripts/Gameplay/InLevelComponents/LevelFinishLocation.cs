using System;
using System.Collections;
using System.Collections.Generic;
using MushiCore.EditorAttributes;
using UnityEngine;

public class LevelFinishLocation : MonoBehaviour
{
    [ColorHeader("Invoking", ColorHeaderColor.InvokingChannels)]
    [ColorHeader("On Level Completed")]
    [SerializeField] private VoidEventChannelSO onLevelCompleted;

    private bool finished = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!finished && other.GetComponentInChildren<ProtagManager>() != null)
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

    private void Update()
    {
        if(Input.GetKey(KeyCode.Z))
            FinishLevel();
    }
}
