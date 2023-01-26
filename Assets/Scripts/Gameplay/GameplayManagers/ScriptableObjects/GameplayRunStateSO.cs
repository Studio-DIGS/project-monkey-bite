using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds state information about the current run
/// </summary>
[CreateAssetMenu(menuName = "Architecture/Gameplay/RunStateScriptableObject")]
public class GameplayRunStateSO : DescriptionBaseSO
{
    [SerializeField] private RunCompletionState runCompletionState;

    public RunCompletionState RunCompletionState => runCompletionState;

    public void Clear()
    {
        runCompletionState = new RunCompletionState();
    }
}

[System.Serializable]
public class RunCompletionState
{
    // Normally put other run stats or information here i guess
    public bool runRequirementsMet = true;
    public bool playerDied = false;
}
