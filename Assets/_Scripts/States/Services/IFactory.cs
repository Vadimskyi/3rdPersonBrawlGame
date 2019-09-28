using System;

namespace Vadimskyi.Utils
{
    public interface IFactory<R, Arg1> : IDisposable
    {
        R Create(Arg1 arg1);
    }
}
