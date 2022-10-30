using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/Input/InputProvider/TestAI")]
public class TestAIInputProvider : PlayerInputProvider
{

    public float angle = 0f;

    public override void Cleanup()
    {
        
    }

    public override PlayerInputState GetInputState()
    {
        var state = new PlayerInputState();
        state.horizontalAxis = Mathf.Cos(angle);
        return state;
    }

    public override void Setup()
    {
        
    }
}
