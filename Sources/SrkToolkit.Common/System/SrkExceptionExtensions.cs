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

namespace System
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Reflection;

    /// <summary>
    /// Extension methods for <see cref="Exception"/>s.
    /// </summary>
    public static class SrkExceptionExtensions
    {
        /// <summary>
        /// Returns a string like "ExceptionType: ExceptionMessage" from an exception.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>a string like "ExceptionType: ExceptionMessage" from the exception or null</returns>
        public static string GetTypeAndMessage(this Exception exception)
        {
            if (exception == null || exception.StackTrace == null)
                return null;

            return exception.GetType().Name + ": " + exception.Message;
        }

        /// <summary>
        /// Summarizes an exception a a string containing the type, the message and the stack trace.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string ToSummarizedString(this Exception exception)
        {
            if (exception == null)
                return null;

            return exception.GetType().Name + ": " + exception.Message + Environment.NewLine + exception.GetCleanStackTrace();
        }

        /// <summary>
        /// Returns a stack trace omitting .NET methods.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string GetCleanStackTrace(this Exception exception)
        {
            if (exception == null || exception.StackTrace == null)
                return null;

            string stack = string.Empty, s = string.Empty;
            bool wasOk = true;
            foreach (var line in stack.Split(new char[] { '\r', '\n' }))
            {
                if (line.Contains("System.") || line.Contains("Microsoft."))
                {
                    if (wasOk)
                        stack += s + line;
                    else
                        stack += s + "  ...";
                    wasOk = false;
                }
                else
                {
                    stack += s + line;
                }

                s = Environment.NewLine;
            }

            return stack;
        }

        /// <summary>
        /// Returns a boolean indicating whether the specified exception is fatal.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>true if the exception is fatal; otherwise, false</returns>
        public static bool IsFatal(this Exception exception)
        {
            while (exception != null)
            {
#if NSTD
                if (exception is OutOfMemoryException)
                {
                    return true;
                }
#else
                if ((exception is OutOfMemoryException && !(exception is InsufficientMemoryException)) ||
                    exception is ThreadAbortException)
                {
                    return true;
                }
#endif

                if (!(exception is TypeInitializationException) && !(exception is TargetInvocationException))
                {
                    break;
                }

                exception = exception.InnerException;
            }

            return false;
        }
    }
}
