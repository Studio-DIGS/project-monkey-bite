using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleStateMachine
{
    public abstract class StateMachine<ContextType>
    {
        // Fields
        protected State<ContextType> currentState;
        private ContextType contextInstance;

        private readonly Dictionary<Type, State<ContextType>> statePool = new();
        private readonly Dictionary<Type, TransitionTable<ContextType>> transitionTablePool = new();
        private float stateEntryTime;

        // Properties
        public State<ContextType> CurrentState => currentState;
        public float CurrentStateDuration => Time.time - stateEntryTime;

        public StateMachine(ContextType contextInstance)
        {
            this.contextInstance = contextInstance;
        }

        public void InitializeEntryState<EntryState>() where EntryState : State<ContextType>, new()
        {
            currentState = GetState<EntryState>();
            stateEntryTime = Time.time;
            currentState.EnterState();
        }

        public void ExitStateMachine()
        {
            currentState.ExitState();
            currentState = null;
        }

        public virtual void Update()
        {
            State<ContextType> currentTransition = null;
            if (currentState.TryTransition(ref currentTransition))
            {
                SwitchStates(currentTransition);
            }

            currentState.UpdateState();
        }

        public virtual void FixedUpdate()
        {
            currentState.FixedUpdateState();
        }

        /// <summary>
        /// Force a transition outside of the standard frame-based transition checks.
        /// </summary>
        /// <param name="state"></param>
        public void ForceTransition(State<ContextType> state)
        {
            SwitchStates(state);
        }

        private void SwitchStates(State<ContextType> state)
        {
            if (currentState == state || state == null) return;
            currentState.ExitState();
            currentState = state;
            stateEntryTime = Time.time;
            currentState.EnterState();
        }

        /// <summary>
        /// Get the state instance of some type from this state machine's table pool, creating a new instance if needed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetState<T>() where T : State<ContextType>, new()
        {
            var type = typeof(T);
            if (statePool.TryGetValue(type, out State<ContextType> state))
            {
                return (T)state;
            }
            else
            {
                var newStateInstance = new T();
                newStateInstance.Initialize(this, contextInstance);
                statePool.Add(type, newStateInstance);
                return newStateInstance;
            }
        }

        /// <summary>
        /// Get the transition table of some type from this state machine's table pool, creating a new instance if needed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetTransitionTable<T>() where T : TransitionTable<ContextType>, new()
        {
            var type = typeof(T);
            if (transitionTablePool.TryGetValue(type, out TransitionTable<ContextType> transitionTable))
            {
                return (T)transitionTable;
            }
            else
            {
                var newTransitionTable = new T();
                newTransitionTable.Initialize(this, contextInstance);
                transitionTablePool.Add(type, newTransitionTable);
                return newTransitionTable;
            }
        }
    }
}