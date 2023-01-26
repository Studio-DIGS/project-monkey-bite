using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class PlayerMovementStateMachine : StateMachine<PlayerBlackboard>
{
    public PlayerMovementStateMachine(PlayerBlackboard blackboardInstance) : base(blackboardInstance)
    {
    }

    protected override void InitializePool()
    {
        AddToPool(new PlayerWalkingState(this));
        AddToPool(new PlayerIdleState(this));
        AddToPool(new PlayerJumpingState(this));
        AddToPool(new PlayerFallingState(this));
    }
}
