using SimpleStateMachine;
using UnityEngine;

public class ProtagGroundMoveSelector : ProtagStateSelector
{
    public ProtagGroundMoveSelector(StateMachine<ProtagBlackboard> stateMachine) : base(stateMachine)
    {
    }

    public override bool TryTransition(ref State<ProtagBlackboard> c)
    {
        throw new System.NotImplementedException();
    }
}
