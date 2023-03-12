using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using MushiCore.EditorAttributes;
using UnityEngine;

[System.Serializable]
public class ProtagBlackboard
{
    [ColorHeader("Dependencies")]
    [SerializeField] public PlayerUserInputProvider inputProvider;
    [SerializeField] public Animator animController;
    [SerializeField] public GameplayLevelStateSO levelState;
        
    [ColorHeader("Camera")]
    [SerializeField] public SimplePathCamera followCamera;
    [SerializeField] public Transform followCameraContainer;
    
    [ColorHeader("Movement")]
    [SerializeField] public PathTransform protagPathTransform;
    [SerializeField] public SimplePathMovement playerSimplePathMovement;
    [SerializeField] public CharacterRotator playerRotator;
    [SerializeField] public PathControllerMotor controllerMotor;

    [ColorHeader("Profiles")]
    [SerializeField] public HorizontalMovementProfile horizontalMovementProfile;
    [SerializeField] public JumpProfile jumpProfile;
    [SerializeField] public FootstoolProfile footstoolProfile;
    [SerializeField] public RollProfile rollProfile;

    [ColorHeader("Debug")]
    // Player state
    [EditorReadOnly] public float coyoteTimer;
    
    public PlayerInputState inputState;

    public void UpdateInputState()
    {
        inputProvider.GetInputState(ref inputState);
        inputProvider.gameplayInputBuffer.RemoveExpired();
    }
}
