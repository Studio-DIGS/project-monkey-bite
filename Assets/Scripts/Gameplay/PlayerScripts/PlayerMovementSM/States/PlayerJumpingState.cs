using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class PlayerJumpingState : State<PlayerBlackboard>
{
    public PlayerJumpingState(StateMachine<PlayerBlackboard> stateMachine) : base(stateMachine)
    {
    }

    public override State<PlayerBlackboard> GetSwitchState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    public override void FixedUpdateState()
    {
        throw new System.NotImplementedException();
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }
}
