using System;
using System.Windows;

namespace SrkToolkit.Mvvm.Tools {

    /// <summary>
    /// Abstraction of a MessageBox.
    /// </summary>
    public interface IMessageBoxService {
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
