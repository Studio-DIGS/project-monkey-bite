using SimpleStateMachine;
using UnityEngine;

public class ProtagUnarmedAttack : ProtagState
{
    public override bool TryTransition(ref State<ProtagBlackboard> c)
    {
        return moveTransitions.ToProtagStateSelector(ref c);
    }

    public override void EnterState()
    {
        Debug.Log("UNARMED ATTACK");
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    public override void FixedUpdateState()
    {
        throw new System.NotImplementedException();
    }
}
