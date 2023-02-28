using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SimpleStateMachine;
using Unity.VisualScripting;
using Debug = UnityEngine.Debug;

public class PlayerStateMachine : StateMachine<ProtagBlackboard>
{
    private static Type[] stateTypes;

    private static Type[] StateTypes
    {
        get
        {
            if (stateTypes == null)
            {
                stateTypes = FindDerivedStateTypes<ProtagState>();
            }
            return stateTypes;
        }
    }
    
    public PlayerStateMachine(ProtagBlackboard blackboardInstance) : base(blackboardInstance)
    {
    }

    protected override void InitializeStatePool()
    {
        AddTypesToStatePool(StateTypes, this);
    }

    protected override void InitializeTransitionPool()
    {
        AddToTransitionPool(new PlayerMovementTransitions(this));
    }
}
