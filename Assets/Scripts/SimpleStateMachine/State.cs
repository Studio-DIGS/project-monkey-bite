using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleStateMachine
{
    public abstract class State<BlkBoard>
    {
        protected StateMachine<BlkBoard> context;
        public float stateEntryTime;
        
        protected BlkBoard blackboard => context.Blackboard;
        
        public State(StateMachine<BlkBoard> stateMachine)
        {
            context = stateMachine;
        }
        
        public abstract bool TryTransition(ref State<BlkBoard> c);
                
        public abstract void EnterState();
        
        public abstract void ExitState();
        
        public abstract void UpdateState();
        
        public abstract void FixedUpdateState();

        protected T GetState<T>() where T : State<BlkBoard>
        {
            return context.GetState<T>();
        }

        protected T GetTransitionTable<T>() where T : TransitionTable<BlkBoard>
        {
            return context.GetTransitionTable<T>();
        }
    }
}
