using System;
using System.Collections;
using System.Collections.Generic;
using MushiCore.EditorAttributes;
using UnityEditor;
using UnityEngine;

public class Protagonist : DescriptionMonoBehavior
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
    }

    public void Initialize(Vector3 pos)
    {
        var pathTransform = blackboard.protagPathTransform;
        blackboard.playerRotator.Initialize(pathTransform);
        blackboard.protagPathTransform.Initialize(blackboard.levelState.levelPath, pos);
        blackboard.controllerMotor.Initialize(blackboard.protagPathTransform);
        blackboard.playerSimplePathMovement.Initialize(blackboard.controllerMotor);
        blackboard.followCamera.Initialize(blackboard.followCameraContainer, pathTransform, pathTransform.transform);
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
        stateMachine.FixedUpdate();
        blackboard.controllerMotor.TickPhysicsBody();
    }

    private void LateUpdate()
    {
        blackboard.followCamera.UpdateCamera();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.Label(blackboard.protagPathTransform.WorldPos + Vector3.up * 2f, currentStateName);
    }
#endif
}
