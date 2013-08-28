
namespace System.Diagnostics
{
    /// <summary>
    /// Extension methods for the <see cref="Stopwatch"/> class.
    /// </summary>
    public static class StopwatchExtensions
    {
        /// <summary>
        /// Starts the <see cref="Stopwatch"/> and returns it for fluent usage.
        /// </summary>
        /// <param name="watch"></param>
        /// <returns>the input <see cref="Stopwatch"/></returns>
        public static Stopwatch StartFluent(this Stopwatch watch)
        {
            watch.Start();
            return watch;
        }

        /// <summary>
        /// Restarts and returns the elapsed time.
        /// </summary>
        /// <param name="watch"></param>
        /// <returns></returns>
        public static TimeSpan GetElapsedAndRestart(this Stopwatch watch)
        {
            var elapsed = watch.Elapsed;
            watch.Restart();
            return elapsed;
        }
    }
}
