using SimpleStateMachine;
using UnityEngine;

public abstract class PlayerMovementState : State<PlayerBlackboard>
{
    protected PlayerMovementState(StateMachine<PlayerBlackboard> stateMachine) : base(stateMachine)
    {
    }

    protected PlayerUserInputProvider inputProvider => blackboard.inputProvider;
    protected SimplePathMovement playerSimplePathMovement => blackboard.playerSimplePathMovement;
    protected MovementContextController movementContextController => blackboard.movementContextController;
    protected SplinePathPhysicsBody pathBody => blackboard.pathBody;
    protected MovementProfileSO movementProfile => blackboard.movementProfile;
    protected PlayerInputState inputState => blackboard.inputState;
    protected PlayerMovementTransitions transitions => GetTransitionTable<PlayerMovementTransitions>();

}
