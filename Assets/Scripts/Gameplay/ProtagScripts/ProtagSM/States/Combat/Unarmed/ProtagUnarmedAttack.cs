using SimpleStateMachine;
using UnityEngine;

public class ProtagUnarmedAttack : ProtagState
{

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
        transitions.ToProtagStateSelector();
    }

    public override void FixedUpdateState()
    {
        throw new System.NotImplementedException();
    }
}
