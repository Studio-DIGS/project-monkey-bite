using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleStateMachine
{
    public abstract class StateMachine<TContext>
    {
        // Fields
        protected State<TContext> currentState;
        private TContext contextInstance;
        private Type currentStateType;

        private readonly Dictionary<Type, State<TContext>> statePool = new();
        private readonly Dictionary<Type, TransitionTable<TContext>> transitionTablePool = new();
        private float stateEntryTime;
        private float stateEntryFixedTime;

        // Properties
        public State<TContext> CurrentState => currentState;
        public float CurrentStateDuration => Time.time - stateEntryTime;
        public float CurrentStateFixedDuration => Time.time - stateEntryFixedTime;

        public StateMachine(TContext contextInstance)
        {
            this.contextInstance = contextInstance;
        }

        public void InitializeEntryState<TEntryState>() where TEntryState : State<TContext>, new()
        {
            var type = typeof(TEntryState);

            currentState = GetState<TEntryState>(type);
            stateEntryTime = Time.time;
            stateEntryFixedTime = Time.fixedTime;
            currentState.EnterState();
        }

        public void ExitStateMachine()
        {
            currentState.ExitState();
            currentState = null;
        }

        public virtual void Update()
        {
            currentState.UpdateState();
        }

        public virtual void FixedUpdate()
        {
            currentState.FixedUpdateState();
        }

        public void TransitionTo<TState>() where TState : State<TContext>, new()
        {
            var type = typeof(TState);
            if (type == currentStateType) return;
            
            currentState?.ExitState();
            currentState = GetState<TState>(type);
            stateEntryTime = Time.time;
            stateEntryFixedTime = Time.fixedTime;
            
            #if UNITY_EDITOR
            
            if(breakPoints.Contains(type))
                Debug.Break();
            
            #endif
            
            currentState.EnterState();
        }

        /// <summary>
        /// Get the state instance of some type from this state machine's table pool, creating a new instance if needed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T GetState<T>(Type type) where T : State<TContext>, new()
        {
            if (statePool.TryGetValue(type, out State<TContext> state))
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
        public T GetTransitionTable<T>() where T : TransitionTable<TContext>, new()
        {
            var type = typeof(T);
            if (transitionTablePool.TryGetValue(type, out TransitionTable<TContext> transitionTable))
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
        
#if UNITY_EDITOR
// Debug Behavior
        
        private HashSet<Type> breakPoints = new();

        public void AddPausePoint(Type type)
        {
            breakPoints.Add(type);
        }

        public void SetPausePoint(Type type)
        {
            breakPoints.Clear();
            AddPausePoint(type);
        }

#endif
    }
}