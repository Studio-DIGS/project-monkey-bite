using System;
using System.Collections;
using System.Collections.Generic;
using MushiCore.EditorAttributes;
using UnityEditor;
using UnityEngine;

public class ProtagManager : DescriptionMonoBehavior
{
    [ColorHeader("Dependencies")]
    [SerializeField] private ProtagBlackboard blackboard;

    // Fields
    private ProtagStateMachine stateMachine;

    [ColorHeader("Debug")]
    [EditorReadOnly, SerializeField] private string currentStateName;

    void Awake()
    {
        stateMachine = new ProtagStateMachine(blackboard);
    }

    private void OnEnable()
    {
        stateMachine.InitializeEntryState<ProtagIdleState>();
        #if UNITY_EDITOR
        //stateMachine.AddPausePoint(typeof(ProtagJumpingState));
        #endif
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
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.Label(blackboard.pathBody.transform.position + Vector3.up * 2f, currentStateName);
    }
#endif
}
