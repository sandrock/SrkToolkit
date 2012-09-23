
namespace System.Windows.Threading
{
    using System;

    public static class DispatcherExtensions
    {
        /// <summary>
        ///   Executes the specified delegate asynchronously with the specified arguments on the thread that the
        ///   System.Windows.Threading.Dispatcher was created on.
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="action">
        ///   The delegate to a method that takes parameters specified in args, which is pushed onto the 
        ///   System.Windows.Threading.Dispatcher event queue.
        /// </param>
        /// <returns>
        ///   An object, which is returned immediately after Overload:System.Windows.Threading.Dispatcher.BeginInvoke
        ///   is called, that can be used to interact with the delegate as it is pending
        ///   execution in the event queue.
        /// </returns>
        public static DispatcherOperation BeginInvoke(this Dispatcher dispatcher, Action action)
        {
            return dispatcher.BeginInvoke(action);
        }

        /// <summary>
        ///   Executes the specified delegate asynchronously with the specified arguments on the thread that the
        ///   System.Windows.Threading.Dispatcher was created on.
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="action">
        ///   The delegate to a method that takes parameters specified in args, which is pushed onto the 
        ///   System.Windows.Threading.Dispatcher event queue.
        /// </param>
        /// <param name="priority">
        ///   The priority, relative to the other pending operations in the System.Windows.Threading.Dispatcher
        /// </param>
        /// <returns>
        ///   An object, which is returned immediately after Overload:System.Windows.Threading.Dispatcher.BeginInvoke
        ///   is called, that can be used to interact with the delegate as it is pending
        ///   execution in the event queue.
        /// </returns>
        public static DispatcherOperation BeginInvoke(this Dispatcher dispatcher, Action action, DispatcherPriority priority)
        {
            return dispatcher.BeginInvoke(action, priority);
        }

        /// <summary>
        ///   Executes the specified delegate asynchronously with the specified arguments on the thread that the
        ///   System.Windows.Threading.Dispatcher was created on.
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="action">
        ///   The delegate to a method that takes parameters specified in args, which is pushed onto the 
        ///   System.Windows.Threading.Dispatcher event queue.
        /// </param>
        /// <param name="priority">
        ///   The priority, relative to the other pending operations in the System.Windows.Threading.Dispatcher
        /// </param>
        /// <returns>
        ///   An object, which is returned immediately after Overload:System.Windows.Threading.Dispatcher.BeginInvoke
        ///   is called, that can be used to interact with the delegate as it is pending
        ///   execution in the event queue.
        /// </returns>
        /// <param name="args">An array of objects to pass as arguments to the given method. Can be null.</param>
        public static DispatcherOperation BeginInvoke(this Dispatcher dispatcher, Action action, DispatcherPriority priority, params object[] args)
        {
            return dispatcher.BeginInvoke(action, priority, args);
        }

        /// <summary>
        ///   Executes the specified delegate asynchronously with the specified arguments on the thread that the
        ///   System.Windows.Threading.Dispatcher was created on.
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="action">
        ///   The delegate to a method that takes parameters specified in args, which is pushed onto the 
        ///   System.Windows.Threading.Dispatcher event queue.
        /// </param>
        /// <returns>
        ///   An object, which is returned immediately after Overload:System.Windows.Threading.Dispatcher.BeginInvoke
        ///   is called, that can be used to interact with the delegate as it is pending
        ///   execution in the event queue.
        /// </returns>
        /// <param name="args">An array of objects to pass as arguments to the given method. Can be null.</param>
        public static DispatcherOperation BeginInvoke(this Dispatcher dispatcher, Action action, params object[] args)
        {
            return dispatcher.BeginInvoke(action, args);
        }
    }
}
