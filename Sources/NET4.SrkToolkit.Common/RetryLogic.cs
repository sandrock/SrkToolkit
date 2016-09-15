
namespace SrkToolkit
{
    using SrkToolkit.Internals;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RetryLogic
    {
        public RetryLogicState State { get; set; }

        private RetryLogic(RetryLogicState state)
        {
            this.State = state;
        }

        static public RetryLogicRunner<TReturn> Do<TReturn>(Func<TReturn> action, RetryLogic retryProfile = null)
        {
            return new RetryLogicRunner<TReturn>(action, retryProfile);
        }

        static public RetryLogicRunner<object> Do(Action action, RetryLogic retryProfile = null)
        {
            return new RetryLogicRunner<object>(action, retryProfile);
        }
#if NET45
        static public RetryLogicRunner<TReturn> DoAsync<TReturn>(Func<Task<TReturn>> action, RetryLogic retryProfile = null)
        {
            return new RetryLogicRunner<TReturn>(action, retryProfile);
        }

        static public RetryLogicRunner<object> DoAsync(Func<Task> action, RetryLogic retryProfile = null)
        {
            return new RetryLogicRunner<object>(action, retryProfile);
        }
#endif

        static public RetryLogic Prepare(RetryLogicState state)
        {
            return new RetryLogic(state);
        }

        static public RetryLogicState InitSettings()
        {
            return new RetryLogicState();
        }
    }
}
