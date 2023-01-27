using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class PlayerWalkingState : State<PlayerBlackboard>
{
    public PlayerWalkingState(StateMachine<PlayerBlackboard> stateMachine) : base(stateMachine)
    {
    }

    public override State<PlayerBlackboard> GetSwitchState()
    {
        if (blackboard.inputState.horizontalAxis == 0)
        {
            return GetState<PlayerIdleState>();
        }

        return null;
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        blackboard.pathController.Move(
            blackboard.movementProfile.moveSpeed *
            /*Time.fixedDeltaTime **/
            blackboard.inputState.horizontalAxis *
            Vector2.right);
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
