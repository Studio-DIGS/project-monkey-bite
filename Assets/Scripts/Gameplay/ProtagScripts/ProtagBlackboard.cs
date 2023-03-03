using System;
using System.Collections;
using System.Collections.Generic;
using MushiCore.EditorAttributes;
using UnityEngine;

public class ProtagBlackboard : DescriptionMonoBehavior
{
    [ColorHeader("Dependencies")]
    [SerializeField] public PlayerUserInputProvider inputProvider;
    [SerializeField] public SimplePathMovement playerSimplePathMovement;
    [SerializeField] public MovementContext movementContext;
    [SerializeField] public SplinePathPhysicsBody pathBody;
    [SerializeField] public CharacterRotator playerRotator;

    [ColorHeader("Profiles")]
    [SerializeField] public HorizontalMovementProfile horizontalMovementProfile;
    [SerializeField] public JumpProfile jumpProfile;
    [SerializeField] public FootstoolProfile footstoolProfile;

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
