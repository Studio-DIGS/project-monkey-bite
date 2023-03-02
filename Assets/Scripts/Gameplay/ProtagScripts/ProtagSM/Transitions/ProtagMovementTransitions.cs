using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagMovementTransitions : TransitionTable<ProtagBlackboard>
{
    private MovementContext movementContext => context.movementContext;
    private MovementProfileSO movementProfile => context.movementProfile;

    public bool ToProtagStateSelector(ref State<ProtagBlackboard> c)
    {
        return GetTransitionTable<ProtagCombatTransitions>().ToCombatSelector(ref c)
               || ToMovementSelector(ref c);
    }
    
    #region Movement Selector
    public bool ToMovementSelector(ref State<ProtagBlackboard> c)
    {
        return ToVerticalActionSelector(ref c)
               || ToDodgeActionSelector(ref c)
               || ToGroundMovementSelector(ref c)
               || ToAirMovementSelector(ref c);
    }
    #endregion

    #region Dodge Action Selector
        
    private bool ToDodgeActionSelector(ref State<ProtagBlackboard> c)
    {
        var buffer = context.inputProvider.GameplayCommandBuffer;
        bool commandFound = buffer.PeekFirstByID(
            (int)PlayerUserInputProvider.PlayerCommandID.DodgeCommandDown,
            out LinkedListNode<GameplayCommandBuffer.GameplayCommand> command);

        bool actionSelected = false;
            
        if (commandFound)
        {
            actionSelected = TrySelectRoll(ref c)
                             || TrySelectDive(ref c);
                
            if(actionSelected)
                buffer.PopCommand(command);
        }

        return actionSelected;
    }
    
    private bool TrySelectRoll(ref State<ProtagBlackboard> c)
    {
        bool grounded = movementContext.IsGrounded;
        if (grounded)
        {
            c = GetState<ProtagRollState>();
            return true;
        }

        return false;
    }
        
    private bool TrySelectDive(ref State<ProtagBlackboard> c)
    {
        bool grounded = movementContext.IsGrounded;
        if (!grounded)
        {
            c = GetState<ProtagDiveState>();
            return true;
        }

        return false;
    }

    #endregion
    
    #region Vertical Action Selector

    public bool ToVerticalActionSelector(ref State<ProtagBlackboard> c)
    {
        var buffer = context.inputProvider.GameplayCommandBuffer;
        bool commandFound = buffer.PeekFirstByID(
            (int)PlayerUserInputProvider.PlayerCommandID.JumpCommandDown,
            out LinkedListNode<GameplayCommandBuffer.GameplayCommand> command);

        bool actionSelected = false;
            
        if (commandFound)
        {
            actionSelected = TrySelectJump(ref c)
                             || TrySelectFootstool(ref c);
                
            if(actionSelected)
                buffer.PopCommand(command);
        }

        return actionSelected;
    }

    private bool TrySelectJump(ref State<ProtagBlackboard> c)
    {
        bool grounded = movementContext.IsGrounded;
        bool coyoteTime = context.coyoteTimer < movementProfile.coyoteTime;
        if (grounded || coyoteTime)
        {
            c = GetState<ProtagJumpingState>();
            return true;
        }

        return false;
    }
        
    private bool TrySelectFootstool(ref State<ProtagBlackboard> c)
    {
        var groundedInfo = movementContext.CheckGroundedOnLayer(movementProfile.footstoolMask);
        if (groundedInfo.surfaceFound)
        {
            c = GetState<ProtagFootstoolJumpingState>();
            return true;
        }

        return false;
    }

    #endregion

    #region Ground Movement Selector

    public bool ToGroundMovementSelector(ref State<ProtagBlackboard> c)
    {
        if (!movementContext.IsGrounded) return false; 
        
        if (context.inputState.horizontalAxis == 0)
        {
            c = GetState<ProtagIdleState>();
            return true;
        }
        else
        {
            c = GetState<ProtagWalkingState>();
            return true;
        }
    }

    #endregion

    #region Air Movement Selector

    public bool ToAirMovementSelector(ref State<ProtagBlackboard> c)
    {
        if (movementContext.IsGrounded) return false;

        c = GetState<ProtagFallingState>();
        
        return true;
    }

    #endregion

}
