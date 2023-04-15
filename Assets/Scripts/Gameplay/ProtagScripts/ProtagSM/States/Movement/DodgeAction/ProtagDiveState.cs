using SimpleStateMachine;
using UnityEngine;

public class ProtagDiveState : ProtagState
{

    public override void EnterState()
    {
        Debug.Log("DIVE");
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
