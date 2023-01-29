using UnityEngine;

namespace SimpleStateMachine
{
    public class TransitionTable<BlkBoard>
    {
        public delegate bool Transition(ref State<BlkBoard> current);
        
        protected StateMachine<BlkBoard> context;
        protected BlkBoard blackboard => context.Blackboard;

        protected State<BlkBoard> GetState<T>() where T : State<BlkBoard>
        {
            return context.GetState<T>();
        }
        
        public TransitionTable(StateMachine<BlkBoard> context)
        {
            this.context = context;
        }
    }
}

