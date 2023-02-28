using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class PlayerMovementTransitions : TransitionTable<ProtagBlackboard>
{
    public PlayerMovementTransitions(StateMachine<ProtagBlackboard> context) : base(context)
    {
    }

    #region GroundedTransitions
    
    /// <summary>
    /// Default transitions for grounded states
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public bool DefaultGroundTransitions(ref State<ProtagBlackboard> c)
    {
        return WhenAirborneToFalling(ref c);
    }

    public bool WhenIdleToIdle(ref State<ProtagBlackboard> c)
    {
        if (blackboard.inputState.horizontalAxis == 0f)
        {
            c = GetState<ProtagIdleState>();
            return true;
        }
        return false;
    }
    
    public bool WhenWalkingToWalking(ref State<ProtagBlackboard> c)
    {
        if (blackboard.inputState.horizontalAxis != 0f)
        {
            c = GetState<ProtagWalkingState>();
            return true;
        }
        return false;
    }

    public bool OnJumpPressedToJump(ref State<ProtagBlackboard> c)
    {
        var buffer = blackboard.inputProvider.GameplayCommandBuffer;
        bool commandFound = buffer.PeekFirstByID(
            (int)PlayerUserInputProvider.PlayerCommandID.JumpCommandDown,
            out LinkedListNode<GameplayCommandBuffer.GameplayCommand> command);
        
        bool grounded = blackboard.movementContext.IsGrounded;
        bool coyoteTime = blackboard.coyoteTimer < blackboard.movementProfile.coyoteTime;
        if (commandFound && (grounded || coyoteTime))
        {
            buffer.PopCommand(command);
            c = GetState<ProtagJumpingState>();
            return true;
        }

        return false;
    }

    public bool OnJumpPressedToFootstoolJump(ref State<ProtagBlackboard> c)
    {
        var buffer = blackboard.inputProvider.GameplayCommandBuffer;
        bool commandFound = buffer.PeekFirstByID(
            (int)PlayerUserInputProvider.PlayerCommandID.JumpCommandDown,
            out LinkedListNode<GameplayCommandBuffer.GameplayCommand> command);
        
        var groundedInfo = blackboard.movementContext.CheckGroundedOnLayer(blackboard.movementProfile.footstoolMask);
        if (commandFound && groundedInfo.surfaceFound)
        {
            buffer.PopCommand(command); 
            c = GetState<ProtagFootstoolJumpingState>();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Transition to Falling State if not grounded
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public bool WhenAirborneToFalling(ref State<ProtagBlackboard> c)
    {
        if (!blackboard.movementContext.IsGrounded)
        {
            c = GetState<ProtagFallingState>();
            return true;
        }
        return false;
    }
    #endregion
    
    #region AirborneTransitions
    
    /// <summary>
    /// Transition to Walk State if Grounded
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public bool WhenGroundedToWalk(ref State<ProtagBlackboard> c)
    {
        if (blackboard.movementContext.IsGrounded)
        {
            c = GetState<ProtagWalkingState>();
            return true;
        }
        return false;
    }

    #endregion
}
