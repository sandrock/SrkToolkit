
namespace System.Windows.Threading
{
    using System.Windows.Threading;

    /// <summary>
    /// Extension methods for the <see cref="Dispatcher"/> class.
    /// </summary>
    public static class DispatcherExtensions
    {
        /// <summary>
        /// Executes the specified delegate asynchronously on the thread that the <see cref="Dispatcher"/> was created on.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <param name="action">The action.</param>
        public static void BeginInvoke(this Dispatcher dispatcher, Action action)
        {
            dispatcher.BeginInvoke(action);
        }

        /// <summary>
        /// Executes the specified delegate asynchronously on the thread that the <see cref="Dispatcher"/> was created on.
        /// </summary>
        public static void BeginInvoke(this Dispatcher dispatcher, Action action, DispatcherPriority priority)
        {
            dispatcher.BeginInvoke(action, priority);
        }

        /// <summary>
        /// Executes the specified delegate synchronously on the thread that the <see cref="Dispatcher"/> was created on.
        /// </summary>
        public static void Invoke(this Dispatcher dispatcher, Action action)
        {
            dispatcher.Invoke(action);
        }

        /// <summary>
        /// Executes the specified delegate synchronously on the thread that the <see cref="Dispatcher"/> was created on.
        /// </summary>
        public static void Invoke(this Dispatcher dispatcher, Action action, DispatcherPriority priority)
        {
            dispatcher.Invoke(action, priority);
        }
    }
}
