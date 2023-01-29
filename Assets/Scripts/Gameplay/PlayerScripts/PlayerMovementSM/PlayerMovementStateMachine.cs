using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class PlayerMovementStateMachine : StateMachine<PlayerBlackboard>
{
    public PlayerMovementStateMachine(PlayerBlackboard blackboardInstance) : base(blackboardInstance)
    {
    }

    protected override void InitializeStatePool()
    {
        AddToStatePool(new PlayerWalkingState(this));
        AddToStatePool(new PlayerIdleState(this));
        AddToStatePool(new PlayerJumpingState(this));
        AddToStatePool(new PlayerFallingState(this));
    }

    protected override void InitializeTransitionPool()
    {
        AddToTransitionPool(new PlayerMovementTransitions(this));
    }
}
