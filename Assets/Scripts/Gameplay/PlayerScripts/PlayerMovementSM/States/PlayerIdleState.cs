using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class PlayerIdleState : State<PlayerBlackboard>
{
    public PlayerIdleState(StateMachine<PlayerBlackboard> stateMachine) : base(stateMachine)
    {
    }

    public override State<PlayerBlackboard> GetSwitchState()
    {
        if (blackboard.inputState.horizontalAxis != 0)
        {
            return GetState<PlayerWalkingState>();
        }
        return null;
    }
    
    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        
    }
}
