using SimpleStateMachine;
using UnityEngine;

public class ProtagRollState : ProtagState
{
    public override bool TryTransition(ref State<ProtagBlackboard> c)
    {
        return moveTransitions.ToProtagStateSelector(ref c);
    }

    public override void EnterState()
    {
        Debug.Log("ROLL");
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
