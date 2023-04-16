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

    public override void UpdateState(float deltaTime)
    {
        transitions.ToProtagStateSelector();
    }

    public override void FixedUpdateState(float fixedDeltaTime)
    {
        
    }
}
