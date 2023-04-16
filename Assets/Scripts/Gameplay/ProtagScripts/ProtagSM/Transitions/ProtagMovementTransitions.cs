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
        bool grounded = controllerMotor.CurrentGroundState.IsStableOnGround;
        if (grounded)
        {
            TransitionTo<ProtagRollState>();
            return true;
        }

        return false;
    }
        
    private bool TrySelectDive()
    {
        bool grounded = controllerMotor.CurrentGroundState.IsStableOnGround;
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
        bool grounded = controllerMotor.CurrentGroundState.IsStableOnGround;
        bool coyoteTime = context.coyoteTimer < context.jumpProfile.coyoteTime;
        if (grounded || coyoteTime)
        {
            TransitionTo<ProtagJumpingState>();
            return true;
        }

        return false;
    }
        
    // @TODO: Re-implement footstooling
    private bool TrySelectFootstool()
    {
        var hits = new RaycastHit[16];
        var groundedInfo = controllerMotor.CapsuleSweep(pathTransform.WPos, Vector3.down, 0.3f,  out RaycastHit hit, footstoolProfile.footstoolMask);
        if (groundedInfo)
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
        if (!controllerMotor.CurrentGroundState.IsStableOnGround) return false; 
        
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
        if (controllerMotor.CurrentGroundState.IsStableOnGround) return false;

        TransitionTo<ProtagFallingState>();
        
        return true;
    }

    #endregion

}
