
namespace System.Diagnostics
{
    /// <summary>
    /// Extension methods for the <see cref="Stopwatch"/> class.
    /// </summary>
    public static class StopwatchExtensions
    {
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
