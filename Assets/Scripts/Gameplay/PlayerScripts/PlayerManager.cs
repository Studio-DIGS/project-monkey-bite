using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : DescriptionMonoBehavior
{
    [ColorHeader("Dependencies")]
    [SerializeField] private PlayerBlackboard blackboard;

    // Fields
    private PlayerMovementStateMachine movementStateMachine;

    [ColorHeader("Debug")]
    [ReadOnly, SerializeField] private string currentStateName;

    void Start()
    {
        movementStateMachine = new PlayerMovementStateMachine(blackboard);
        movementStateMachine.InitializeEntryState<PlayerIdleState>();
    }
    
    void Update() 
    {
        blackboard.UpdateInputState();
        movementStateMachine.Update();
        currentStateName = movementStateMachine.CurrentState.GetType().ToString();
    }

    private void FixedUpdate()
    {
        blackboard.movementContextController.UpdateContext();
        movementStateMachine.FixedUpdate();
    }
}
