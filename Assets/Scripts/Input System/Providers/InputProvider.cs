using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public interface InputProvider<TInputState, TInputEvents>
{
    public abstract TInputState GetInputState(TInputState initial);

    public TInputEvents Events
    {
        get;
    }
}
