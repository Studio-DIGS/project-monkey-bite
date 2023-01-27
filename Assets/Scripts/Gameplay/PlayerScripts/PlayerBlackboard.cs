using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackboard : DescriptionMonoBehavior
{
    [ColorHeader("Dependencies")]
    [SerializeField] public PlayerUserInputProvider inputProvider;
    [SerializeField] public SimplePathMovement playerSimplePathMovement;
    [SerializeField] public MovementContextController movementContextController;
    [SerializeField] public SplinePathPhysicsBody pathBody;
    [SerializeField] public MovementProfileSO movementProfile;
    
    [ReadOnly] public PlayerInputState inputState;
    
    // Player state
    
    

    public void UpdateInputState()
    {
        inputProvider.GetInputState(ref inputState);
    }
}
