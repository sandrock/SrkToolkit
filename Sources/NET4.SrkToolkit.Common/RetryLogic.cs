
namespace SrkToolkit
{
    using SrkToolkit.Internals;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class RetryLogic
    {
        ////public RetryLogicState State { get; set; }

        ////private RetryLogic(RetryLogicState state)
        ////{
        ////    this.State = state;
        ////}

        static public RetryLogicState<TReturn> Do<TReturn>(Func<TReturn> action)
        {
            return new RetryLogicState<TReturn>(action);
        }

        static public RetryLogicState<object> Do(Action action)
        {
            return new RetryLogicState<object>(action);
        }
#if NET45
        static public RetryLogicState<TReturn> DoAsync<TReturn>(Func<Task<TReturn>> action)
        {
            return new RetryLogicState<TReturn>(action);
        }

        static public RetryLogicState<object> DoAsync(Func<Task> action)
        {
            return new RetryLogicState<object>(action);
        }
#endif

        ////static public RetryLogic Prepare(RetryLogicState state)
        ////{
        ////    return new RetryLogic(state);
        ////}

        static public RetryLogicState<Nothing> BeginPrepare()
        {
            return RetryLogicState<Nothing>.BeginPrepare();
        }
    }
}
