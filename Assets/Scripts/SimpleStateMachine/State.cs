namespace SimpleStateMachine
{
    public abstract class State<TContext>
    {
        protected StateMachine<TContext> stateMachine;
        protected TContext context;

        public State()
        {
        }

        public virtual void Initialize(StateMachine<TContext> stateMachine, TContext context)
        {
            this.stateMachine = stateMachine;
            this.context = context;
        }
        
        public abstract void EnterState();

        public abstract void ExitState();

        public abstract void UpdateState();

        public abstract void FixedUpdateState();

        protected TTransitions GetTransitionTable<TTransitions>() where TTransitions : TransitionTable<TContext>, new()
            => stateMachine.GetTransitionTable<TTransitions>();
    }
}