namespace SrkToolkit.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    public class RetryLogicState<TReturn>
    {
        private Func<TReturn> actionToExecute;
        private Func<Task<TReturn>> actionToExecuteAsync;

        private IList<Action<TReturn>> onSuccessCallbacks;

        private IList<Type> exceptions;

        private int numberOfAttempts;
        private TimeSpan intervalDelay;
        private RetryIntervalStrategy intervalStrategy;
        private int scaleFactor;
        private bool traceRetryEvents;
        private bool onFailureContinue;

        private IDictionary<Type, Action<Exception>> onExceptionCallbacks;
        private IList<Action> onFailureCallbacks;
        private bool isFrozen;
        private bool isPreparing;
        
        internal RetryLogicState(Func<TReturn> action)
            : this()
        {
            this.actionToExecute = action;
        }

        internal RetryLogicState(Action action)
            : this()
        {
            this.actionToExecute = new Func<TReturn>(() =>
            {
                action();
                return default(TReturn);
            });
        }

#if NET45
        internal RetryLogicState(Func<Task<TReturn>> action)
            : this()
        {
            this.actionToExecuteAsync = action;
        }

        internal RetryLogicState(Func<Task> action)
            : this()
        {
            actionToExecuteAsync = new Func<Task<TReturn>>(async () =>
            {
                await action();
                return default(TReturn);
            });
        }
#endif

        private RetryLogicState()
        {
            // set default values
            this.numberOfAttempts = 3;
            this.intervalDelay = TimeSpan.FromMilliseconds(2000);
            this.intervalStrategy = RetryIntervalStrategy.LinearDelay;
            this.scaleFactor = 10;
            this.traceRetryEvents = false;
            this.onFailureContinue = false;

            this.exceptions = new List<Type>();
            this.onExceptionCallbacks = new Dictionary<Type, Action<Exception>>();
            this.onFailureCallbacks = new List<Action>();
            this.onSuccessCallbacks = new List<Action<TReturn>>();
        }

        //TODO : plug it
        ////public TReturn Result { get; set; }

        ////public int ExceptionsCatched { get; set; }

        ////public bool Succeed { get; set; }

        internal static RetryLogicState<Nothing> BeginPrepare()
        {
            var state = new RetryLogicState<Nothing>();
            state.isPreparing = true;
            return state;
        }

        public RetryLogicState<TReturn> Handle<TException>() where TException : Exception
        {
            this.exceptions.Add(typeof(TException));
            return this;
        }

        public RetryLogicState<TReturn> AtMost(int numberOfAttempts)
        {
            this.CheckIsNotFrozen();
            this.numberOfAttempts = numberOfAttempts;
            return this;
        }

        public RetryLogicState<TReturn> WithConstantInterval(TimeSpan interval)
        {
            this.CheckIsNotFrozen();
            this.intervalDelay = interval;
            this.intervalStrategy = RetryIntervalStrategy.ConstantDelay;
            return this;
        }
        public RetryLogicState<TReturn> WithLinearInterval(TimeSpan interval, int scaleFactor)
        {
            this.CheckIsNotFrozen(); 
            this.intervalDelay = interval;
            this.intervalStrategy = RetryIntervalStrategy.LinearDelay;
            this.scaleFactor = scaleFactor;
            return this;
        }
        public RetryLogicState<TReturn> WithProgressiveInterval(TimeSpan interval, int scaleFactor)
        {
            this.CheckIsNotFrozen();
            this.intervalDelay = interval;
            this.intervalStrategy = RetryIntervalStrategy.ProgressiveDelay;
            this.scaleFactor = scaleFactor;
            return this;
        }
        public RetryLogicState<TReturn> WithExponentialInterval(TimeSpan interval, int scaleFactor)
        {
            this.CheckIsNotFrozen();
            this.intervalDelay = interval;
            this.intervalStrategy = RetryIntervalStrategy.ExponentialDelay;
            this.scaleFactor = scaleFactor;
            return this;
        }

        public RetryLogicState<TReturn> OnException<TException>(Action<TException> onExceptionCallback) where TException : Exception
        {
            this.CheckIsNotFrozen();
            this.onExceptionCallbacks.Add(typeof(TException), new Action<Exception>(e => onExceptionCallback((TException)e)));
            return this;
        }

        public RetryLogicState<TReturn> OnFailure(Action onFailureCallback)
        {
            this.CheckIsNotFrozen();
            this.onFailureCallbacks.Add(onFailureCallback);
            return this;
        }

        public RetryLogicState<TReturn> WithTrace(bool traceRetryEvents)
        {
            this.CheckIsNotFrozen();
            this.traceRetryEvents = traceRetryEvents;
            return this;
        }

        [Obsolete("Preview feature: not ready for production!")]
        public RetryLogicState<TReturn> OnFailureContinue()
        {
            this.CheckIsNotFrozen();
            this.onFailureContinue = true; ;
            return this;
        }

#if NET45
        public async Task<TReturn> RunAsync()
        {
            this.CheckIsRunable();

            if (this.actionToExecuteAsync == null)
            {
                throw new InvalidOperationException("no action to execute defined");
            }

            if (this.exceptions == null || this.exceptions.Count <= 0)
            {
                throw new InvalidOperationException("no exceptions to handle defined");
            }

            bool processSuceed = false;
            int count = 0;
            TReturn result = default(TReturn);

        TryProcess:

            try
            {
                ++count;
                TraceIfNeeded("Attempt #" + count.ToString(), false);
                result = await this.actionToExecuteAsync();
                processSuceed = true;
                TraceIfNeeded("Success !", false);
            }
            catch (Exception ex)
            {
                TraceIfNeeded("Exception catched: ", true, ex);
                var isHandled = false;
                foreach (var handledException in this.exceptions)
                {
                    if (ex.GetType().IsAssignableFrom(handledException))
                    {
                        isHandled = true;
                        break;
                    }
                }

                if (isHandled)
                {
                    TraceIfNeeded("Exception handled", false);

                    if (this.onExceptionCallbacks != null && this.onExceptionCallbacks.Count > 0)
                    {
                        foreach (var callback in this.onExceptionCallbacks)
                        {
                            if (ex.GetType().IsAssignableFrom(callback.Key))
                            {
                                callback.Value(ex);
                            }
                        }
                    }

                    if (count < this.numberOfAttempts)
                    {
                        Thread.Sleep(GetDelay(count, intervalDelay, intervalStrategy, scaleFactor));

                        TraceIfNeeded("Retry", false);

                        goto TryProcess;
                    }
                    else
                    {
                        TraceIfNeeded("Max attempt number reached", false);

                        foreach (var callback in this.onFailureCallbacks)
                        {
                            this.onFailureContinue = true;
                            callback();
                        }

                        if (!this.onFailureContinue)
                        {
                            throw ex;
                        }
                        else
                        {
                            result = default(TReturn);
                        }
                    }
                }
                else
                {
                    TraceIfNeeded("Exception not handled", true);

                    ////foreach (var callback in this.onFailureCallbacks)
                    ////{
                    ////    callback();
                    ////}
                    throw ex;
                }
            }

            if (processSuceed)
            {
                foreach (var callback in this.onSuccessCallbacks)
                {
                    callback(result);
                }
            }

            return result;
        }
#endif

        public TReturn Run()
        {
            this.CheckIsRunable();

            if (this.actionToExecute == null)
            {
                throw new InvalidOperationException("no action to execute defined");
            }

            if (this.exceptions == null || this.exceptions.Count <= 0)
            {
                throw new InvalidOperationException("no exceptions to handle defined");
            }

            bool processSuceed = false;
            int count = 0;
            TReturn result = default(TReturn);

        TryProcess:

            try
            {
                ++count;
                TraceIfNeeded("Attempt #" + count.ToString(), false);
                result = this.actionToExecute();
                processSuceed = true;
                TraceIfNeeded("Success !", false);
            }
            catch (Exception ex)
            {
                TraceIfNeeded("Exception catched: ", true, ex);
                var isHandled = false;
                foreach (var handledException in this.exceptions)
                {
                    if (ex.GetType().IsAssignableFrom(handledException))
                    {
                        isHandled = true;
                        break;
                    }
                }

                if (isHandled)
                {
                    TraceIfNeeded("Exception handled", false);

                    if (this.onExceptionCallbacks != null && this.onExceptionCallbacks.Count > 0)
                    {
                        foreach (var callback in this.onExceptionCallbacks)
                        {
                            if (ex.GetType().IsAssignableFrom(callback.Key))
                            {
                                callback.Value(ex);
                            }
                        }
                    }

                    if (count < this.numberOfAttempts)
                    {
                        Thread.Sleep(GetDelay(count, intervalDelay, intervalStrategy, scaleFactor));

                        TraceIfNeeded("Retry", false);

                        goto TryProcess;
                    }
                    else
                    {
                        TraceIfNeeded("Max attempt number reached", false);

                        foreach (var callback in this.onFailureCallbacks)
                        {
                            this.onFailureContinue = true;
                            callback();
                        }

                        if (!this.onFailureContinue)
                        {
                            throw ex;
                        }
                        else
                        {
                            result = default(TReturn);
                        }
                    }
                }
                else
                {
                    TraceIfNeeded("Exception not handled", true);

                    ////foreach (var callback in this.onFailureCallbacks)
                    ////{
                    ////    callback();
                    ////}
                    throw ex;
                }
            }

            if (processSuceed)
            {
                foreach (var callback in this.onSuccessCallbacks)
                {
                    callback(result);
                }
            }

            return result;
        }


        public RetryLogicState<TReturn> EndPrepare()
        {
            this.isFrozen = true;
            this.isPreparing = false;
            return this;
        }

        public RetryLogicState<TReturn> OnSuccess(Action<TReturn> onSuccessCallback)
        {
            this.CheckIsNotFrozen();
            this.onSuccessCallbacks.Add(onSuccessCallback);
            return this;
        }

        public RetryLogicState<TReturn1> Do<TReturn1>(Func<TReturn1> action)
        {
            ////return new RetryLogicState<TReturn>(action, this.Clone());
            return CloneSync<TReturn, TReturn1>(action, this);
        }

        public RetryLogicState<Nothing> Do(Action action)
        {
            ////return new RetryLogicState<object>(action, this.Clone());
            var function = new Func<Nothing>(() =>
            {
                action();
                return default(Nothing);
            });
            return CloneSync<TReturn, Nothing>(function, this);
        }

#if NET45
        public RetryLogicState<TReturn1> DoAsync<TReturn1>(Func<Task<TReturn1>> action)
        {
            ////return new RetryLogicState<TReturn>(action, this.Clone());
            return CloneAsync<TReturn, TReturn1>(action, this);
        }

        public RetryLogicState<Nothing> DoAsync(Func<Task> action)
        {
            ////return new RetryLogicState<object>(action, this.Clone());
            var function = new Func<Task<Nothing>>(async () =>
            {
                await action();
                return default(Nothing);
            });
            return CloneAsync<TReturn, Nothing>(function, this);
        }
#endif

        internal void CheckIsNotFrozen()
        {
            if (this.isFrozen)
            {
                throw new InvalidOperationException("Is frozen");
            }
        }

        internal void CheckIsRunable()
        {
            if (this.isPreparing)
            {
                throw new InvalidOperationException("Is preparing");
            }
        }

        internal RetryLogicState<TReturnTo> Clone<TReturnTo>(RetryLogicState<TReturnTo> target)
        {
            target.numberOfAttempts = this.numberOfAttempts;
            target.intervalDelay = this.intervalDelay;
            target.intervalStrategy = this.intervalStrategy;
            target.scaleFactor = this.scaleFactor;
            target.traceRetryEvents = this.traceRetryEvents;
            target.onFailureContinue = this.onFailureContinue;
            target.exceptions = new List<Type>(this.exceptions);
            target.onExceptionCallbacks = new Dictionary<Type, Action<Exception>>(this.onExceptionCallbacks.Count);
            target.onFailureCallbacks = new List<Action>(this.onFailureCallbacks.Count);
            ////target.isFrozen = this.isFrozen; // do not clone dat
            target.isPreparing = this.isPreparing;

            foreach (var pair in this.onExceptionCallbacks)
            {
                target.onExceptionCallbacks.Add(pair.Key, pair.Value);
            }

            this.onFailureCallbacks.ToList().ForEach(c => target.onFailureCallbacks.Add(c));
            return target;
        }

        private TimeSpan GetDelay(int attemptIndex, TimeSpan intervalDelay, RetryIntervalStrategy strategy, int scaleFactor)
        {
            if (attemptIndex < 1)
            {
                throw new IndexOutOfRangeException("Attempt index should not be < 1");
            }

            double factor = 1.0;
            switch (strategy)
            {
                case RetryIntervalStrategy.ConstantDelay:
                    return intervalDelay;
                case RetryIntervalStrategy.LinearDelay:
                    factor = (attemptIndex - 1) * scaleFactor > 1 ? (attemptIndex - 1) * scaleFactor : 1;
                    return TimeSpan.FromMilliseconds(factor + intervalDelay.TotalMilliseconds);
                case RetryIntervalStrategy.ProgressiveDelay:
                    factor = (attemptIndex - 1) * scaleFactor > 1 ? (attemptIndex - 1) * scaleFactor : 1;
                    return TimeSpan.FromMilliseconds(factor * intervalDelay.TotalMilliseconds);
                case RetryIntervalStrategy.ExponentialDelay:
                    factor = Math.Pow(attemptIndex - 1, scaleFactor);
                    factor = factor > 1 ? factor : 1;
                    return TimeSpan.FromMilliseconds(factor * intervalDelay.TotalMilliseconds);
                default:
                    break;
            }

            throw new ArgumentException("strategy should be defined");
        }

        private void TraceIfNeeded(string message, bool isError, Exception exception = null)
        {
            if (this.traceRetryEvents)
            {
                if(isError)
                {
                    if (exception != null)
                    {
                        Trace.TraceError(message + Environment.NewLine + exception.ToString() + Environment.NewLine);
                    }
                    else
                    {
                        Trace.TraceError(message + Environment.NewLine);
                    }
                }
                else{
                    Trace.TraceInformation(message + Environment.NewLine);
                }
            }
        }

#if NET45
        private static RetryLogicState<TReturnTo> CloneAsync<TReturnFrom, TReturnTo>(Func<Task<TReturnTo>> action, RetryLogicState<TReturnFrom> source)
        {
            var target = new RetryLogicState<TReturnTo>(action);
            target = source.Clone<TReturnTo>(target);
            return target;
        }
#endif

        private static RetryLogicState<TReturnTo> CloneSync<TReturnFrom, TReturnTo>(Func<TReturnTo> action, RetryLogicState<TReturnFrom> source)
        {
            var target = new RetryLogicState<TReturnTo>(action);
            target = source.Clone<TReturnTo>(target);
            return target;
        }

        
    }
}
