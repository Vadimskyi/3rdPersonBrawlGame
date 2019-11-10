using System;

namespace Vadimskyi.Utils
{
    public interface IFactory<R, Arg1> : IDisposable
    {
        R Create(Arg1 arg1);
    }
    public interface IFactory<R, Arg1, Arg2> : IDisposable
    {
        R Create(Arg1 arg1, Arg2 arg2);
    }
}
