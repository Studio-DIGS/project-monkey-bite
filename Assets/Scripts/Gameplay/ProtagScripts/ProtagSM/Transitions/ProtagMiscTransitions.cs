using SimpleStateMachine;
using UnityEngine;

public partial class ProtagTransitions : TransitionTable<ProtagBlackboard>
{
    private MovementContext movementContext => context.movementContext;
    private FootstoolProfile footstoolProfile => context.footstoolProfile;
    private GameplayInputBuffer buffer => context.inputProvider.gameplayInputBuffer;

    public bool ToProtagStateSelector()
    {
        return ToCombatSelector()
               || ToMovementSelector();
    }
}
