using SimpleStateMachine;
using UnityEngine;

public abstract class ProtagState : State<ProtagBlackboard>
{
    protected PlayerUserInputProvider inputProvider => context.inputProvider;
    protected SimplePathMovement playerSimplePathMovement => context.playerSimplePathMovement;
    protected MovementContext movementContext => context.movementContext;
    protected SplinePathPhysicsBody pathBody => context.pathBody;
    protected MovementProfileSO movementProfile => context.movementProfile;
    protected PlayerInputState inputState => context.inputState;
    protected ProtagMovementTransitions moveTransitions => GetTransitionTable<ProtagMovementTransitions>();
    protected ProtagCombatTransitions combatTransitions => GetTransitionTable<ProtagCombatTransitions>();

}