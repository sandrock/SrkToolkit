
namespace SrkToolkit
{
    using SrkToolkit.Internals;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class RetryLogic
    {
        static public RetryLogicState<TReturn> Do<TReturn>(Func<TReturn> action)
        {
            return new RetryLogicState<TReturn>(action);
        }

        static public RetryLogicState<Nothing> Do(Action action)
        {
            return new RetryLogicState<Nothing>(action);
        }
#if NET45
        static public RetryLogicState<TReturn> DoAsync<TReturn>(Func<Task<TReturn>> action)
        {
            return new RetryLogicState<TReturn>(action);
        }

        static public RetryLogicState<Nothing> DoAsync(Func<Task> action)
        {
            return new RetryLogicState<Nothing>(action);
        }
#endif

        static public RetryLogicState<Nothing> BeginPrepare()
        {
            return RetryLogicState<Nothing>.BeginPrepare();
        }
    }
}
