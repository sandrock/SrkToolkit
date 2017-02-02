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

    /// <summary>
    /// Abstraction of a MessageBox.
    /// </summary>
    public interface IMessageBoxService
    {
        /// <summary>
        /// Displays a message box that contains the specified text and an OK button.
        /// </summary>
        /// <param name="messageBoxText">The message to display.</param>
        /// <returns>System.Windows.MessageBoxResult.OK in all cases.</returns>
        MessageBoxResult Show(string messageBoxText);

        /// <summary>
        /// Displays a message box that contains the specified text, title bar caption, and response buttons.
        /// </summary>
        /// <param name="messageBoxText">The message to display.</param>
        /// <param name="caption">The title of the message box.</param>
        /// <param name="button">A value that indicates the button or buttons to display.</param>
        /// <returns>A value that indicates the user's response to the message.</returns>
        MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button);

        /// <summary>
        /// Displays a message box that contains the specified text and an OK button only when debugging.
        /// </summary>
        /// <param name="messageBoxText">The message to display.</param>
        /// <returns>System.Windows.MessageBoxResult.OK in all cases.</returns>
        MessageBoxResult ShowDebug(string messageBoxText);
    }
}
