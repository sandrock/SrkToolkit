namespace SrkToolkit.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class RetryLogicRunner<TReturn>
    {
        private Func<TReturn> actionToExecute;
        private Func<Task<TReturn>> actionToExecuteAsync;

        private IList<Action<TReturn>> onSuccessCallbacks;

        private RetryLogicState state;


        public TReturn Result { get; set; }

        public int ExceptionsCatched { get; set; }

        public bool Succeed { get; set; }

        public RetryLogicRunner(Func<TReturn> action, RetryLogic retryProfile)
            : this(retryProfile)
        {
            this.actionToExecute = action;
        }

        public RetryLogicRunner(Func<TReturn> action, RetryLogicState retryProfile)
            : this(retryProfile)
        {
            this.actionToExecute = action;
        }

        public RetryLogicRunner(Action action, RetryLogic retryProfile)
            : this(retryProfile)
        {
            this.actionToExecute = new Func<TReturn>(() =>
            {
                action();
                return default(TReturn);
            });
        }

        public RetryLogicRunner(Action action, RetryLogicState retryProfile)
            : this(retryProfile)
        {
            this.actionToExecute = new Func<TReturn>(() =>
            {
                action();
                return default(TReturn);
            });
        }

#if NET45
        public RetryLogicRunner(Func<Task<TReturn>> action, RetryLogic retryProfile)
            : this(retryProfile)
        {
            this.actionToExecuteAsync = action;
        }

        public RetryLogicRunner(Func<Task<TReturn>> action, RetryLogicState retryProfile)
            : this(retryProfile)
        {
            this.actionToExecuteAsync = action;
        }

        public RetryLogicRunner(Func<Task> action, RetryLogic retryProfile)
            : this(retryProfile)
        {
            actionToExecuteAsync = new Func<Task<TReturn>>(async () =>
            {
                await action();
                return default(TReturn);
            });
        }

        public RetryLogicRunner(Func<Task> action, RetryLogicState retryProfile)
            : this(retryProfile)
        {
            actionToExecuteAsync = new Func<Task<TReturn>>(async () =>
            {
                await action();
                return default(TReturn);
            });
        }
#endif
        private RetryLogicRunner(RetryLogic retryProfile)
        {
            if (retryProfile == null)
            {
                this.state = new RetryLogicState();
            }
            else
            {
                this.state = retryProfile.State.Clone();
            }

            this.onSuccessCallbacks = new List<Action<TReturn>>();
        }

        private RetryLogicRunner(RetryLogicState retryProfile)
        {
            if (retryProfile == null)
                throw new ArgumentNullException("retryProfile");

            this.state = retryProfile;

            this.onSuccessCallbacks = new List<Action<TReturn>>();
        }

        public RetryLogicRunner<TReturn> Handle<TException>() where TException : Exception
        {
            this.state.Handle<TException>();
            return this;
        }

        public RetryLogicRunner<TReturn> NumberOfAttempts(int numberOfAttempts)
        {
            this.state.CheckIsNotFrozen();
            this.state.NumberOfAttempts(numberOfAttempts);
            return this;
        }

        public RetryLogicRunner<TReturn> IntervalStrategy(RetryIntervalStrategy strategy, int scaleFactor)
        {
            this.state.IntervalStrategy(strategy, scaleFactor);
            return this;
        }

        public RetryLogicRunner<TReturn> WithInterval(TimeSpan interval)
        {
            this.state.WithInterval(interval);
            return this;
        }

        public RetryLogicRunner<TReturn> OnException<TException>(Action<TException> onExceptionCallback) where TException : Exception
        {
            this.state.OnException<TException>( onExceptionCallback);
            return this;
        }

        public RetryLogicRunner<TReturn> OnFailure(Action onFailureCallback)
        {
            this.state.OnFailure(onFailureCallback);
            return this;
        }

        public RetryLogicRunner<TReturn> OnSuccess(Action<TReturn> onSuccessCallback)
        {
            this.onSuccessCallbacks.Add(onSuccessCallback);
            return this;
        }

        public RetryLogicRunner<TReturn> TraceRetryEvents(bool traceRetryEvents)
        {
            this.state.TraceRetryEvents(traceRetryEvents);
            return this;
        }

        public RetryLogicRunner<TReturn> ThrowIfNotHandled(bool throwIfNotHandled)
        {
            this.state.ThrowIfNotHandled( throwIfNotHandled);
            return this;
        }

#if NET45
        public async Task<TReturn> RunAsync()
        {
            return await this.state.RunAsync<TReturn>(this.actionToExecuteAsync, this.onSuccessCallbacks);
        }
#endif

        public TReturn Run()
        {
            return this.state.Run<TReturn>(this.actionToExecute, this.onSuccessCallbacks);
        }
    }

}
