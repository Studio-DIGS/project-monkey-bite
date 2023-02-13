using System;
using System.Collections;
using System.Collections.Generic;
using MushiCore.EditorAttributes;
using UnityEngine;

public class PlayerManager : DescriptionMonoBehavior
{
    [ColorHeader("Dependencies")]
    [SerializeField] private PlayerBlackboard blackboard;

    // Fields
    private PlayerMovementStateMachine movementStateMachine;

    [ColorHeader("Debug")]
    [EditorReadOnly, SerializeField] private string currentStateName;

    void Start()
    {
        movementStateMachine = new PlayerMovementStateMachine(blackboard);
        movementStateMachine.InitializeEntryState<PlayerIdleState>();
    }
    
    void Update() 
    {
        blackboard.UpdateInputState();
        movementStateMachine.Update();
        blackboard.playerRotator.AlignDirection(blackboard.inputState.horizontalAxis);
        
        // Debug
        currentStateName = movementStateMachine.CurrentState.GetType().ToString();
    }

    private void FixedUpdate()
    {
        blackboard.movementContextController.UpdateContext();
        movementStateMachine.FixedUpdate();
    }
}
