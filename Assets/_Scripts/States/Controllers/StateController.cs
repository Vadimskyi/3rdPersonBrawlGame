using System;
using UniRx;

namespace Vadimskyi.Utils
{
    public abstract class StateController : IDisposable
    {
        public event Action<IState> OnStateChanged = delegate { };
        public IState CurrentState { get; protected set; }
        public IState PreviousState { get; protected set; }

        protected StateController()
        {
            Observable.EveryUpdate().Subscribe(_ => Update());
        }

        public virtual void SetState(IState nextState, object args = null)
        {
            CurrentState?.OnStateExit();
            PreviousState = CurrentState;
            CurrentState = nextState;
            CurrentState?.OnStateEnter(args);
            OnStateChanged?.Invoke(CurrentState);
        }

        public void Update()
        {
            CurrentState?.OnUpdate();
        }
        
        public void Dispose()
        {
            CurrentState?.OnStateExit();
        }
    }
}