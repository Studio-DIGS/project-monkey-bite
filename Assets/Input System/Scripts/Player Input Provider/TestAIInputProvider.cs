using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/Input/InputProvider/TestAI")]
public class TestAIInputProvider : PlayerInputProvider
{

    public float angle = 0f;

    public override PlayerInputState GetInputState()
    {
        var state = new PlayerInputState();
        Vector2 screenSize = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        state.mousePosition = screenSize / 2 + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * screenSize / 5;
        return state;
    }
}
