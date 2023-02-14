using SimpleStateMachine;
using UnityEngine;

public class PlayerMovementTransitions : TransitionTable<PlayerBlackboard>
{
    public PlayerMovementTransitions(StateMachine<PlayerBlackboard> context) : base(context)
    {
    }

    #region GroundedTransitions
    
    /// <summary>
    /// Default transitions for grounded states
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public bool DefaultGroundTransitions(ref State<PlayerBlackboard> c)
    {
        return WhenAirborneToFalling(ref c);
    }

    /// <summary>
    /// Add a force transition to Jump when jump input is pressed
    /// </summary>
    public void AddOnJumpPressedToJump()
    {
        blackboard.inputProvider.Events.OnJumpPressed += OnJumpPressedToJump;
    }

    /// <summary>
    /// Remove a force transition to Jump when jump input is pressed
    /// </summary>
    public void RemoveOnJumpPressedToJump()
    {
        blackboard.inputProvider.Events.OnJumpPressed -= OnJumpPressedToJump;
    }
    
    private void OnJumpPressedToJump()
    {
        bool grounded = blackboard.movementContextController.IsGrounded;
        bool coyoteTime = blackboard.coyoteTimer < blackboard.movementProfile.coyoteTime;
        if(grounded || coyoteTime)
            context.ForceTransition(GetState<PlayerJumpingState>());
    }

    /// <summary>
    /// Add a force transition to Footstool Jump when jump input is pressed
    /// </summary>
    public void AddOnJumpPressedToFootstoolJump()
    {
        blackboard.inputProvider.Events.OnJumpPressed += OnJumpPressedToFootstoolJump;
    }

    /// <summary>
    /// Remove a force transition to Footstool Jump when jump input is pressed
    /// </summary>
    public void RemoveOnJumpPressedToFootstoolJump()
    {
        blackboard.inputProvider.Events.OnJumpPressed -= OnJumpPressedToFootstoolJump;
    }

    private void OnJumpPressedToFootstoolJump()
    {
        var groundedInfo = blackboard.movementContextController.CheckGroundedOnLayer(blackboard.movementProfile.footstoolMask);
        bool coyoteTime = blackboard.coyoteTimer < blackboard.movementProfile.coyoteTime;
        if(groundedInfo.surfaceFound || coyoteTime)
            context.ForceTransition(GetState<PlayerFootstoolJumpingState>());
    }

    /// <summary>
    /// Transition to Falling State if not grounded
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public bool WhenAirborneToFalling(ref State<PlayerBlackboard> c)
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
    
    /// <summary>
    /// Transition to Walk State if Grounded
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public bool WhenGroundedToWalk(ref State<PlayerBlackboard> c)
    {
        if (blackboard.movementContextController.IsGrounded)
        {
            c = GetState<PlayerWalkingState>();
            return true;
        }
        return false;
    }

    #endregion
}
