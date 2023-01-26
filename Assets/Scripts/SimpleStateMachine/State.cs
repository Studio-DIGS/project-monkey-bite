using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleStateMachine
{
    public abstract class State<BlkBoard>
    {
        protected StateMachine<BlkBoard> context;
        protected BlkBoard blackboard => context.Blackboard;
        
        public State(StateMachine<BlkBoard> stateMachine)
        {
            context = stateMachine;
        }
        
        public abstract State<BlkBoard> GetSwitchState();
                
        public abstract void EnterState();
        
        public abstract void ExitState();
        
        public abstract void UpdateState();
        
        public abstract void FixedUpdateState();

        protected State<BlkBoard> GetState<T>() where T : State<BlkBoard>
        {
            return context.GetPooledState<T>();
        }
    }
}
