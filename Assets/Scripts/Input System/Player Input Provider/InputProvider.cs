using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public abstract class InputProvider<InputState> : ScriptableObject
{
    protected InputState inputState;
    public abstract InputState GetInputState();
}
