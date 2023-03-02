using SimpleStateMachine;
using UnityEngine;

public class ProtagArmedAttack : ProtagState
{
    public override bool TryTransition(ref State<ProtagBlackboard> c)
    {
        return moveTransitions.ToProtagStateSelector(ref c);
    }

    public override void EnterState()
    {
        Debug.Log("ARMED ATTACK YEEHAW");
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        
    }

    public override void FixedUpdateState()
    {
        
    }
}
