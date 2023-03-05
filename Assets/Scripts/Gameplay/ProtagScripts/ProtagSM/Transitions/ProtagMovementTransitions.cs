using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public partial class ProtagTransitions : TransitionTable<ProtagBlackboard>
{

    #region Movement Selector
    public bool ToMovementSelector()
    {
        return ToVerticalActionSelector()
               || ToDodgeActionSelector()
               || ToGroundMovementSelector()
               || ToAirMovementSelector();
    }
    #endregion

    #region Dodge Action Selector
        
    private bool ToDodgeActionSelector()
    {
        bool commandFound = buffer.PeekFirstByID(
            (int)PlayerUserInputProvider.PlayerCommandID.DodgeCommandDown,
            out LinkedListNode<GameplayInputBuffer.GameplayCommand> command);

        bool actionSelected = false;
            
        if (commandFound)
        {
            actionSelected = TrySelectRoll()
                             || TrySelectDive();
                
            if(actionSelected)
                buffer.PopCommand(command);
        }

        return actionSelected;
    }
    
    private bool TrySelectRoll()
    {
        bool grounded = movementContext.IsGrounded;
        if (grounded)
        {
            TransitionTo<ProtagRollState>();
            return true;
        }

        return false;
    }
        
    private bool TrySelectDive()
    {
        bool grounded = movementContext.IsGrounded;
        if (!grounded)
        {
            TransitionTo<ProtagDiveState>();
            return true;
        }

        return false;
    }

    #endregion
    
    #region Vertical Action Selector

    public bool ToVerticalActionSelector()
    {
        bool commandFound = buffer.PeekFirstByID(
            (int)PlayerUserInputProvider.PlayerCommandID.JumpCommandDown,
            out LinkedListNode<GameplayInputBuffer.GameplayCommand> command);

        bool actionSelected = false;
            
        if (commandFound)
        {
            actionSelected = TrySelectJump()
                             || TrySelectFootstool();
                
            if(actionSelected)
                buffer.PopCommand(command);
        }

        return actionSelected;
    }

    private bool TrySelectJump()
    {
        bool grounded = movementContext.IsGrounded;
        bool coyoteTime = context.coyoteTimer < context.jumpProfile.coyoteTime;
        if (grounded || coyoteTime)
        {
            TransitionTo<ProtagJumpingState>();
            return true;
        }

        return false;
    }
        
    private bool TrySelectFootstool()
    {
        var groundedInfo = movementContext.CheckGroundedOnLayer(footstoolProfile.footstoolMask);
        if (groundedInfo.surfaceFound)
        {
            TransitionTo<ProtagFootstoolJumpingState>();
            return true;
        }

        return false;
    }

    #endregion

    #region Ground Movement Selector

    public bool ToGroundMovementSelector()
    {
        if (!movementContext.IsGrounded) return false; 
        
        if (context.inputState.horizontalAxis == 0)
        {
            TransitionTo<ProtagIdleState>();
            return true;
        }
        else
        {
            TransitionTo<ProtagWalkingState>();
            return true;
        }
    }

    #endregion

    #region Air Movement Selector

    public bool ToAirMovementSelector()
    {
        if (movementContext.IsGrounded) return false;

        TransitionTo<ProtagFallingState>();
        
        return true;
    }

    #endregion

}
