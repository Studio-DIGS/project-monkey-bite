using SimpleStateMachine;
using UnityEngine;

public class PlayerMovementTransitions : TransitionTable<PlayerBlackboard>
{
    public PlayerMovementTransitions(StateMachine<PlayerBlackboard> context) : base(context)
    {
    }

    #region GroundedTransitions
    public bool DefaultGroundTransitions(ref State<PlayerBlackboard> c)
    {
        return OnInputToJump(ref c) || OnAirborneToFalling(ref c);
    }
    
    private bool OnInputToJump(ref State<PlayerBlackboard> c)
    {
        if (blackboard.inputState.jumpPressed)
        {
            c = GetState<PlayerJumpingState>();
            return true;
        }
        return false;
    }

    private bool OnAirborneToFalling(ref State<PlayerBlackboard> c)
    {
        if (!blackboard.movementContextController.IsGrounded)
        {
            c = GetState<PlayerFallingState>();
            return true;
        }
        return false;
    }
    #endregion
    
    #region AirborneTransitions
    
    public bool OnGroundedToWalk(ref State<PlayerBlackboard> c)
    {
        if (blackboard.movementContextController.IsGrounded)
        {
            c = GetState<PlayerWalkingState>();
            return true;
        }
        return false;
    }
    
    public bool OnInputToCoyoteTimeJump(ref State<PlayerBlackboard> c)
    {
        if (blackboard.coyoteTimer < blackboard.movementProfile.coyoteTime && blackboard.inputState.jumpPressed)
        {
            c = GetState<PlayerJumpingState>();
            return true;
        }
        return false;
    }
    
    #endregion
}
