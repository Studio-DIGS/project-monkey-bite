using SimpleStateMachine;
using UnityEngine;

public class ProtagCombatSelector : ProtagStateSelector
{
    public ProtagCombatSelector(StateMachine<ProtagBlackboard> stateMachine) : base(stateMachine)
    {
    }

    public override bool TryTransition(ref State<ProtagBlackboard> c)
    {
        return false;
    }
}
