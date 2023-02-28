using SimpleStateMachine;
using UnityEngine;

public abstract class ProtagState : State<ProtagBlackboard>
{
    protected ProtagState(StateMachine<ProtagBlackboard> stateMachine) : base(stateMachine)
    {
    }

    protected PlayerUserInputProvider inputProvider => blackboard.inputProvider;
    protected SimplePathMovement playerSimplePathMovement => blackboard.playerSimplePathMovement;
    protected MovementContext movementContext => blackboard.movementContext;
    protected SplinePathPhysicsBody pathBody => blackboard.pathBody;
    protected MovementProfileSO movementProfile => blackboard.movementProfile;
    protected PlayerInputState inputState => blackboard.inputState;
    protected PlayerMovementTransitions transitions => GetTransitionTable<PlayerMovementTransitions>();

}

public abstract class ProtagStateSelector : StateSelector<ProtagBlackboard>
{
    protected ProtagStateSelector(StateMachine<ProtagBlackboard> stateMachine) : base(stateMachine)
    {
    }

    protected PlayerUserInputProvider inputProvider => blackboard.inputProvider;
    protected SimplePathMovement playerSimplePathMovement => blackboard.playerSimplePathMovement;
    protected MovementContext movementContext => blackboard.movementContext;
    protected SplinePathPhysicsBody pathBody => blackboard.pathBody;
    protected MovementProfileSO movementProfile => blackboard.movementProfile;
    protected PlayerInputState inputState => blackboard.inputState;
    protected PlayerMovementTransitions transitions => GetTransitionTable<PlayerMovementTransitions>();

}
