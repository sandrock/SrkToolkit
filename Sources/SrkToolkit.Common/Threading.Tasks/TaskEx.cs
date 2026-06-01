
namespace SrkToolkit.Threading.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for the <see cref="Task"/> class.
    /// </summary>
    public static class TaskEx
    {
        /// <summary>
        /// Runs the specified action and returns the task for a fluent usage.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The associated task</returns>
        public static Task Run(Action action)
        {
            var task = new Task(action);
            task.Start();
            return task;
        }
    }
}
