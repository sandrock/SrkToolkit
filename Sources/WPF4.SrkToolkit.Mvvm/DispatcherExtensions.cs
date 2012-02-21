using System.Windows.Threading;

namespace System.Windows.Threading
{
    public static class DispatcherExtensions
    {
        public static void BeginInvoke(this Dispatcher dispatcher, Action action)
        {
            dispatcher.BeginInvoke(action);
        }

        public static void BeginInvoke(this Dispatcher dispatcher, Action action, DispatcherPriority priority)
        {
            dispatcher.BeginInvoke(action, priority);
        }

        public static void Invoke(this Dispatcher dispatcher, Action action)
        {
            dispatcher.Invoke(action);
        }

        public static void Invoke(this Dispatcher dispatcher, Action action, DispatcherPriority priority)
        {
            dispatcher.Invoke(action, priority);
        }
    }
}
