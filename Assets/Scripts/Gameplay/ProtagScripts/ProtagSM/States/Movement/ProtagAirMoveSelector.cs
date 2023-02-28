using SimpleStateMachine;
using UnityEngine;

public class ProtagAirMoveSelector : ProtagStateSelector
{
    public ProtagAirMoveSelector(StateMachine<ProtagBlackboard> stateMachine) : base(stateMachine)
    {
    }

    public override bool TryTransition(ref State<ProtagBlackboard> c)
    {
        throw new System.NotImplementedException();
    }
}
