namespace SimpleStateMachine
{
    public abstract class State<ContextType>
    {
        protected StateMachine<ContextType> stateMachine;
        protected ContextType context;

        public State()
        {
        }

        public virtual void Initialize(StateMachine<ContextType> stateMachine, ContextType context)
        {
            this.stateMachine = stateMachine;
            this.context = context;
        }

        public abstract bool TryTransition(ref State<ContextType> c);

        public abstract void EnterState();

        public abstract void ExitState();

        public abstract void UpdateState();

        public abstract void FixedUpdateState();

        protected T GetState<T>() where T : State<ContextType>, new()
            => stateMachine.GetState<T>();

        protected T GetTransitionTable<T>() where T : TransitionTable<ContextType>, new()
            => stateMachine.GetTransitionTable<T>();
    }
}