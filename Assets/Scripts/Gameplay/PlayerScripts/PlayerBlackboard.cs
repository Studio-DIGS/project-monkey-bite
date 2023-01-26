using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackboard : MonoBehaviour
{
    [SerializeField] public MovementProfileSO movementProfile;
    [SerializeField] public PlayerUserInputProvider inputProvider;
    [SerializeField] public PathController pathController;

    [ReadOnly] public PlayerInputState inputState;

    public void UpdateInputState()
    {
        inputProvider.GetInputState(ref inputState);
    }
}
