using UnityEngine;

namespace SimpleStateMachine
{
    /// <summary>
    /// A special type of state that does not implement behavior, but immediately switches to another state.
    /// Intended as a transition-only implementation of hierarchy, to reduce transition hell.
    /// </summary>
    /// <typeparam name="BlkBoard"></typeparam>
    public abstract class StateSelector<BlkBoard> : State<BlkBoard>
    {
        public StateSelector(StateMachine<BlkBoard> stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState()
        {
            State<BlkBoard> selectedState = null;
            if (!TryTransition(ref selectedState))
            {
                Debug.LogError($"State Selector {this} did not select a state, which is invalid behavior");
            }
            context.ForceTransition(selectedState);
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
}