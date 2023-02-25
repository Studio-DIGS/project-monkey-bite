using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SimpleStateMachine;
using Unity.VisualScripting;
using Debug = UnityEngine.Debug;

public class PlayerMovementStateMachine : StateMachine<PlayerBlackboard>
{
    private static Type[] stateTypes;

    private static Type[] StateTypes
    {
        get
        {
            if (stateTypes == null)
            {
                stateTypes = FindDerivedStateTypes<PlayerMovementState>();
            }
            return stateTypes;
        }
    }
    
    public PlayerMovementStateMachine(PlayerBlackboard blackboardInstance) : base(blackboardInstance)
    {
    }

    protected override void InitializeStatePool()
    {

        //var stopwatch = Stopwatch.StartNew();
        /*
        AddToStatePool(new PlayerWalkingState(this));
        AddToStatePool(new PlayerIdleState(this));
        AddToStatePool(new PlayerJumpingState(this));
        AddToStatePool(new PlayerFootstoolJumpingState(this));
        AddToStatePool(new PlayerFallingState(this));
        */
        
        AddTypesToStatePool(StateTypes, this);
        
        //Debug.Log(stopwatch.ElapsedMilliseconds);
    }

    protected override void InitializeTransitionPool()
    {
        AddToTransitionPool(new PlayerMovementTransitions(this));
    }
}
