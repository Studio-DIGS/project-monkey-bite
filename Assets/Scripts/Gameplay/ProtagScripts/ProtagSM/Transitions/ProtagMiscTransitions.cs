using KinematicCharacterController;
using SimpleStateMachine;
using UnityEngine;

public partial class ProtagTransitions : TransitionTable<ProtagBlackboard>
{
    private FootstoolProfile footstoolProfile => context.footstoolProfile;
    private GameplayInputBuffer buffer => context.inputProvider.gameplayInputBuffer;

    private PathControllerMotor controllerMotor => context.controllerMotor;
    private PathTransform pathTransform => context.protagPathTransform;

    public bool ToProtagStateSelector()
    {
        return ToCombatSelector()
               || ToMovementSelector();
    }
}
