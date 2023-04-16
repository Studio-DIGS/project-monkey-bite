using SimpleStateMachine;
using UnityEngine;

public class ProtagArmedAttack : ProtagState
{
    public override void EnterState()
    {
        Debug.Log("ARMED ATTACK");
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
