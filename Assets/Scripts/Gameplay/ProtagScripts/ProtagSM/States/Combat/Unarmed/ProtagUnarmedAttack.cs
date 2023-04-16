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

    public override void UpdateState(float deltaTime)
    {
        transitions.ToProtagStateSelector();
    }

    public override void FixedUpdateState(float fixedDeltaTime)
    {
        throw new System.NotImplementedException();
    }
}
