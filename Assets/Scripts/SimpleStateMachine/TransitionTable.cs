namespace SimpleStateMachine
{
    /// <summary>
    /// Instanced transition table for reusable transition logic
    /// </summary>
    /// <typeparam name="ContextType"></typeparam>
    public class TransitionTable<ContextType>
    {
        protected StateMachine<ContextType> stateMachine;
        protected ContextType context;

        protected TransitionTable()
        {
        }

        public virtual void Initialize(StateMachine<ContextType> stateMachine, ContextType context)
        {
            this.stateMachine = stateMachine;
            this.context = context;
        }

        protected T GetState<T>() where T : State<ContextType>, new() => stateMachine.GetState<T>();
        protected T GetTransitionTable<T>() where T : TransitionTable<ContextType>, new() => stateMachine.GetTransitionTable<T>();
    }
}