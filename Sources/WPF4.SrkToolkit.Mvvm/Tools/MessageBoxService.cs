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

namespace SrkToolkit.Mvvm.Tools
{
    using System;
    using System.Windows;
    using System.Diagnostics;

    /// <summary>
    /// Abstraction of the MessageBox component. 
    /// </summary>
    public class MessageBoxService : IMessageBoxService
    {
        private readonly Func<string, string, MessageBoxButton, MessageBoxResult> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBoxService"/> class using the real MessageBox implementation.
        /// </summary>
        public MessageBoxService()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBoxService"/> class using a fake MessageBox implementation.
        /// </summary>
        /// <param name="action">The action.</param>
        public MessageBoxService(Func<string, string, MessageBoxButton, MessageBoxResult> action)
        {
            this.action = action;
        }

        /// <summary>
        /// Displays a message box that contains the specified text and an OK button only when debugging.
        /// </summary>
        /// <param name="messageBoxText">The message to display.</param>
        /// <returns>
        /// System.Windows.MessageBoxResult.OK in all cases.
        /// </returns>
        [DebuggerStepThrough]
        public MessageBoxResult ShowDebug(string messageBoxText)
        {
#if DEBUG
            if (action != null)
            {
                return action(messageBoxText, null, MessageBoxButton.OK);
            }
            else
            {
                return MessageBox.Show(messageBoxText);
            }
#else
            return MessageBoxResult.OK;
#endif
        }

        /// <summary>
        /// Displays a message box that contains the specified text and an OK button.
        /// </summary>
        /// <param name="messageBoxText">The message to display.</param>
        /// <returns>
        /// System.Windows.MessageBoxResult.OK in all cases.
        /// </returns>
        [DebuggerStepThrough]
        public MessageBoxResult Show(string messageBoxText)
        {
            if (action != null)
            {
                return action(messageBoxText, null, MessageBoxButton.OK);
            }
            else
            {
                return MessageBox.Show(messageBoxText);
            }
        }

        /// <summary>
        /// Displays a message box that contains the specified text, title bar caption, and response buttons.
        /// </summary>
        /// <param name="messageBoxText">The message to display.</param>
        /// <param name="caption">The title of the message box.</param>
        /// <param name="button">A value that indicates the button or buttons to display.</param>
        /// <returns>
        /// A value that indicates the user's response to the message.
        /// </returns>
        [DebuggerStepThrough]
        public MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            if (action != null)
            {
                return action(messageBoxText, caption, button);
            }
            else
            {
                return MessageBox.Show(messageBoxText, caption, button);
            }
        }
    }
}
