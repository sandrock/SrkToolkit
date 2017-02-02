// 
// Copyright 2014 SandRock
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

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
