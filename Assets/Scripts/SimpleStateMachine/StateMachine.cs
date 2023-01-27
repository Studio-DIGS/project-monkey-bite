using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleStateMachine
{
    public abstract class StateMachine<BlkBoard>
    {
        // Fields
        protected State<BlkBoard> currentState;
        private Dictionary<System.Type, State<BlkBoard>> statePool = new();
        private BlkBoard blackboardInstance;

        // Properties
        public State<BlkBoard> CurrentState => currentState;
        public BlkBoard Blackboard => blackboardInstance;

        public StateMachine(BlkBoard blackboardInstance)
        {
            this.blackboardInstance = blackboardInstance;
            InitializePool();
        }

        protected abstract void InitializePool();

        public void AddToPool<T>(T instance) where T : State<BlkBoard>
        {
            statePool.TryAdd(typeof(T), instance);
        }

        public void InitializeEntryState<EntryState>() where EntryState : State<BlkBoard>
        {
            currentState = GetPooledState<EntryState>();
            currentState.EnterState();
        }

        public virtual void Update()
        {
            var res = currentState.GetSwitchState();
            if (res != null)
            {
                SwitchStates(res);
            }
            currentState.UpdateState();
        }
        
        public virtual void FixedUpdate()
        {
            currentState.FixedUpdateState();
        }

        private void SwitchStates(State<BlkBoard> state)
        {
            if (currentState == state || state == null) return;
            currentState.ExitState();
            currentState = state;
            currentState.EnterState();
        }

        public State<BlkBoard> GetPooledState<T>() where T : State<BlkBoard>
        {
            var type = typeof(T);
            if (statePool.TryGetValue(type, out State<BlkBoard> state))
            {
                return state;
            }
            else
            {
                return null;
            }
        }
    }
}

