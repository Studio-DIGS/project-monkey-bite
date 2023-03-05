using SimpleStateMachine;
using UnityEngine;

public class ProtagThrowAttack : ProtagState
{
    public override void EnterState()
    {
        Debug.Log("THROW ATTACK");
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        transitions.ToProtagStateSelector();
    }

    public override void FixedUpdateState()
    {
        
    }
}
