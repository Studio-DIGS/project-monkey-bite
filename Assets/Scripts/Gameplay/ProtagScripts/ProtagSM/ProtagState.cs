
using SimpleStateMachine;
using UnityEngine;

public abstract class ProtagState : State<ProtagBlackboard>
{
    protected ProtagTransitions transitions;
    
    public override void Initialize(StateMachine<ProtagBlackboard> stateMachine, ProtagBlackboard context)
    {
        base.Initialize(stateMachine, context);
        transitions = GetTransitionTable<ProtagTransitions>();
    }

    protected PlayerUserInputProvider inputProvider => context.inputProvider;
    protected SimplePathMovement playerSimplePathMovement => context.playerSimplePathMovement;
    protected PathControllerMotor controllerMotor => context.controllerMotor;
    protected HorizontalMovementProfile hMoveProfile => context.horizontalMovementProfile;
    protected FootstoolProfile footstoolProfile => context.footstoolProfile;
    protected JumpProfile jumpProfile => context.jumpProfile;
    protected RollProfile rollProfile => context.rollProfile;
    protected CombatProfile combatProfile => context.combatProfile;
    protected PlayerInputState inputState => context.inputState;
    protected Animator animationController => context.animController;
    
    // Utility functions
    protected void GroundStop(float fixedDeltaTime)
    {
        Vector2 groundNormal = controllerMotor.CurrentGroundState.GroundNormal;
        playerSimplePathMovement.SimpleGroundedHorizontalMovement(
            0, 
            hMoveProfile.groundedWalkVel,
            hMoveProfile.groundedWalkAccel,
            hMoveProfile.groundedFriction,
            fixedDeltaTime, 
            groundNormal);
    }

    protected void AlignCharacter()
    {
        context.playerRotator.AlignDirection(context.inputState.horizontalAxis);
    }
}