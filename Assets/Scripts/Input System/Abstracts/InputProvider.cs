using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Floofy.Core.InputSystem
{
    public interface InputProvider<TInputState>
    {
        public abstract TInputState GetInputState(TInputState initial);
    }
}
