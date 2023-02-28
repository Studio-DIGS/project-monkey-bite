using SimpleStateMachine;
using UnityEngine;

public class ProtagMovementSelector : ProtagStateSelector
{
    public ProtagMovementSelector(StateMachine<ProtagBlackboard> stateMachine) : base(stateMachine)
    {
    }

    public override bool TryTransition(ref State<ProtagBlackboard> c)
    {
        if (movementContext.IsGrounded)
        {
            c = GetState<ProtagGroundMoveSelector>();
            return true;
        }
        else
        {
            c = GetState<ProtagAirMoveSelector>();
            return true;
        }
    }
}
