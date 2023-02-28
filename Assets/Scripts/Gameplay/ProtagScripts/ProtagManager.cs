using System;
using System.Collections;
using System.Collections.Generic;
using MushiCore.EditorAttributes;
using UnityEngine;

public class ProtagManager : DescriptionMonoBehavior
{
    [ColorHeader("Dependencies")]
    [SerializeField] private ProtagBlackboard blackboard;

    // Fields
    private PlayerStateMachine stateMachine;

    [ColorHeader("Debug")]
    [EditorReadOnly, SerializeField] private string currentStateName;

    void Awake()
    {
        stateMachine = new PlayerStateMachine(blackboard);
    }

    private void OnEnable()
    {
        stateMachine.InitializeEntryState<ProtagIdleState>();
    }

    private void OnDisable() 
    {
        stateMachine.ExitStateMachine();
    }

    void Update() 
    {
        blackboard.UpdateInputState();
        stateMachine.Update();
        blackboard.playerRotator.AlignDirection(blackboard.inputState.horizontalAxis);
        
        // Debug
        currentStateName = stateMachine.CurrentState.GetType().ToString();
    }

    private void FixedUpdate()
    {
        blackboard.movementContext.UpdateContext();
        stateMachine.FixedUpdate();
    }
}
