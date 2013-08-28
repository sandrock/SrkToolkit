
namespace SrkToolkit.Threading.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class TaskEx
    {
        public static Task Run(Action action)
        {
            var task = new Task(action);
            task.Start();
            return task;
        }
    }
}
