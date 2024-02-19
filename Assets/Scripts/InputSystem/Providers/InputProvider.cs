using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public interface InputProvider<TInputState, TInputEvents>
{
    public abstract void GetInputState(ref TInputState initial);

    public TInputEvents Events
    {
        get;
    }
}
