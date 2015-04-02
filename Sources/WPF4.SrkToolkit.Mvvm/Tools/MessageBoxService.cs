using System;
using System.Windows;
using System.Diagnostics;

namespace SrkToolkit.Mvvm.Tools {

    /// <summary>
    /// Abstraction of the MessageBox component. 
    /// </summary>
    public class MessageBoxService : IMessageBoxService {

        private readonly Func<string, string, MessageBoxButton, MessageBoxResult> action;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBoxService"/> class using the real MessageBox implementation.
        /// </summary>
        public MessageBoxService() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBoxService"/> class using a fake MessageBox implementation.
        /// </summary>
        /// <param name="action">The action.</param>
        public MessageBoxService(Func<string, string, MessageBoxButton, MessageBoxResult> action) {
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
        public MessageBoxResult ShowDebug(string messageBoxText) {
#if DEBUG
            if (action != null) {
                return action(messageBoxText, null, MessageBoxButton.OK);
            } else {
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
        public MessageBoxResult Show(string messageBoxText) {
            if (action != null) {
                return action(messageBoxText, null, MessageBoxButton.OK);
            } else {
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
            if (action != null) {
                return action(messageBoxText, caption, button);
            } else {
                return MessageBox.Show(messageBoxText, caption, button);
            }
        }

    }
}
