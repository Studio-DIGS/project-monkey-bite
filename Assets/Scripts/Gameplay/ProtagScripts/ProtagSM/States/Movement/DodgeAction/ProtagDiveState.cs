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

    public override void UpdateState()
    {
        transitions.ToProtagStateSelector();
    }

    public override void FixedUpdateState()
    {
        
    }
}
