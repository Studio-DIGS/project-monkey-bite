namespace SimpleStateMachine
{
    /// <summary>
    /// Instanced transition table for reusable transition logic
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class TransitionTable<TContext>
    {
        protected StateMachine<TContext> stateMachine;
        protected TContext context;

        protected TransitionTable()
        {
        }

        public virtual void Initialize(StateMachine<TContext> stateMachine, TContext context)
        {
            this.stateMachine = stateMachine;
            this.context = context;
        }

        protected void TransitionTo<TState>() where TState : State<TContext>, new() => stateMachine.TransitionTo<TState>();
        protected TTransitions GetTransitionTable<TTransitions>() where TTransitions : TransitionTable<TContext>, new() => stateMachine.GetTransitionTable<TTransitions>();
    }
}