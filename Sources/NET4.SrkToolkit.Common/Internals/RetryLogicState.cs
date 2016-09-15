namespace SrkToolkit.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    public class RetryLogicState
    {
        private IList<Type> exceptions;

        private int numberOfAttempts;
        private TimeSpan intervalDelay;
        private RetryIntervalStrategy intervalStrategy;
        private int scaleFactor;
        private bool traceRetryEvents;
        private bool throwIfNotHandled;

        private IDictionary<Type, Action<Exception>> onExceptionCallbacks;
        private IList<Action> onFailureCallbacks;
        private bool isFrozen;

        public RetryLogicState()
        {
            // set default values
            this.numberOfAttempts = 3;
            this.intervalDelay = TimeSpan.FromMilliseconds(2000);
            this.intervalStrategy = RetryIntervalStrategy.LinearDelay;
            this.scaleFactor = 10;
            this.traceRetryEvents = false;
            this.throwIfNotHandled = false;

            this.exceptions = new List<Type>();
            this.onExceptionCallbacks = new Dictionary<Type, Action<Exception>>();
            this.onFailureCallbacks = new List<Action>();
        }

        public RetryLogicState Handle<TException>() where TException : Exception
        {
            this.exceptions.Add(typeof(TException));
            return this;
        }

        public RetryLogicState NumberOfAttempts(int numberOfAttempts)
        {
            this.numberOfAttempts = numberOfAttempts;
            return this;
        }

        public RetryLogicState IntervalStrategy(RetryIntervalStrategy strategy, int scaleFactor)
        {
            this.intervalStrategy = strategy;
            this.scaleFactor = scaleFactor;
            return this;
        }

        public RetryLogicState WithInterval(TimeSpan interval)
        {
            this.intervalDelay = interval;
            return this;
        }

        public RetryLogicState OnException<TException>(Action<TException> onExceptionCallback) where TException : Exception
        {
            this.onExceptionCallbacks.Add(typeof(TException), new Action<Exception>(e => onExceptionCallback((TException)e)));
            return this;
        }

        public RetryLogicState OnFailure(Action onFailureCallback)
        {
            this.onFailureCallbacks.Add(onFailureCallback);
            return this;
        }

        public RetryLogicState TraceRetryEvents(bool traceRetryEvents)
        {
            this.traceRetryEvents = traceRetryEvents;
            return this;
        }

        public RetryLogicState ThrowIfNotHandled(bool throwIfNotHandled)
        {
            this.throwIfNotHandled = throwIfNotHandled;
            return this;
        }

#if NET45
        public async Task<TReturn> RunAsync<TReturn>(Func<Task<TReturn>> actionToExecuteAsync, IList<Action<TReturn>> onSuccessCallbacks)
        {
            if (actionToExecuteAsync == null)
            {
                throw new NullReferenceException("no action to execute defined");
            }

            if (this.exceptions == null || this.exceptions.Count <= 0)
            {
                throw new NullReferenceException("no exceptions to handle defined");
            }

            bool processSuceed = false;
            int count = 0;
            TReturn result = default(TReturn);

        TryProcess:

            try
            {
                ++count;
                TraceIfNeeded("Attempt #" + count.ToString(), false);
                result = await actionToExecuteAsync();
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
                            callback();
                        }

                        if (this.throwIfNotHandled)
                        {
                            throw ex;
                        }
                    }
                }
                else
                {
                    TraceIfNeeded("Exception not handled", true);

                    foreach (var callback in this.onFailureCallbacks)
                    {
                        callback();
                    }
                    throw ex;
                }
            }

            if (processSuceed)
            {
                foreach (var callback in onSuccessCallbacks)
                {
                    callback(result);
                }
            }

            return result;
        }
#endif

        public TReturn Run<TReturn>(Func<TReturn> actionToExecute, IList<Action<TReturn>> onSuccessCallbacks)
        {
            if (actionToExecute == null)
            {
                throw new NullReferenceException("no action to execute defined");
            }

            if (this.exceptions == null || this.exceptions.Count <= 0)
            {
                throw new NullReferenceException("no exceptions to handle defined");
            }

            bool processSuceed = false;
            int count = 0;
            TReturn result = default(TReturn);

        TryProcess:

            try
            {
                ++count;
                TraceIfNeeded("Attempt #" + count.ToString(), false);
                result = actionToExecute();
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
                            callback();
                        }

                        if (this.throwIfNotHandled)
                        {
                            throw ex;
                        }
                    }
                }
                else
                {
                    TraceIfNeeded("Exception not handled", true);

                    foreach (var callback in this.onFailureCallbacks)
                    {
                        callback();
                    }
                    throw ex;
                }
            }

            if (processSuceed)
            {
                foreach (var callback in onSuccessCallbacks)
                {
                    callback(result);
                }
            }

            return result;
        }

        
        public RetryLogicState Prepare()
        {
            this.isFrozen = true;
            return this;
        }

        internal void CheckIsNotFrozen()
        {
            if (this.isFrozen)
            {
                throw new InvalidOperationException("Is frozen");
            }
        }

        ////public RetryLogic Prepare()
        ////{
        ////    return RetryLogic.Prepare(this.Clone());
        ////}

        internal RetryLogicState Clone()
        {
            var result = new RetryLogicState()
            {
                numberOfAttempts = this.numberOfAttempts,
                intervalDelay = this.intervalDelay,
                intervalStrategy = this.intervalStrategy,
                scaleFactor = this.scaleFactor,
                traceRetryEvents = this.traceRetryEvents,
                throwIfNotHandled = this.throwIfNotHandled,
                exceptions = new List<Type>(this.exceptions),
                onExceptionCallbacks = new Dictionary<Type, Action<Exception>>(this.onExceptionCallbacks.Count),
                onFailureCallbacks = new List<Action>(this.onFailureCallbacks.Count),
            };

            foreach (var pair in this.onExceptionCallbacks)
            {
                result.onExceptionCallbacks.Add(pair.Key, pair.Value);
            }

            this.onFailureCallbacks.ToList().ForEach(c => result.onFailureCallbacks.Add(c));
            return result;
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

        public RetryLogicRunner<TReturn> Do<TReturn>(Func<TReturn> action)
        {
            return new RetryLogicRunner<TReturn>(action, this.Clone());
        }

        public RetryLogicRunner<object> Do(Action action)
        {
            return new RetryLogicRunner<object>(action, this.Clone());
        }

#if NET45
        public RetryLogicRunner<TReturn> DoAsync<TReturn>(Func<Task<TReturn>> action)
        {
            return new RetryLogicRunner<TReturn>(action, this.Clone());
        }

        public RetryLogicRunner<object> DoAsync(Func<Task> action)
        {
            return new RetryLogicRunner<object>(action, this.Clone());
        }
#endif
    }
}
