using System;

namespace Vadimskyi.Utils
{
    public interface IState : IDisposable
    {
        void OnUpdate();
        void OnStateEnter(object args = null);
        void OnStateExit();
    }
}