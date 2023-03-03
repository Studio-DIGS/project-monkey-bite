using SimpleStateMachine;
using UnityEngine;

public abstract class ProtagState : State<ProtagBlackboard>
{
    protected PlayerUserInputProvider inputProvider => context.inputProvider;
    protected SimplePathMovement playerSimplePathMovement => context.playerSimplePathMovement;
    protected MovementContext movementContext => context.movementContext;
    protected SplinePathPhysicsBody pathBody => context.pathBody;
    protected HorizontalMovementProfile hMoveProfile => context.horizontalMovementProfile;
    protected FootstoolProfile footstoolProfile => context.footstoolProfile;
    protected JumpProfile jumpProfile => context.jumpProfile;
    protected RollProfile rollProfile => context.rollProfile;
    protected PlayerInputState inputState => context.inputState;
    protected ProtagMovementTransitions moveTransitions => GetTransitionTable<ProtagMovementTransitions>();
    protected ProtagCombatTransitions combatTransitions => GetTransitionTable<ProtagCombatTransitions>();

}