using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SimpleStateMachine;
using Unity.VisualScripting;

public class ProtagStateMachine : StateMachine<ProtagBlackboard>
{
    public ProtagStateMachine(ProtagBlackboard contextInstance) : base(contextInstance)
    {
    }
}
