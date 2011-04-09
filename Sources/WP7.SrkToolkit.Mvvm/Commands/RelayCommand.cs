using System;
using System.Windows.Input;
using System.Diagnostics;

namespace SrkToolkit.Mvvm.Commands {

    /// <summary>
    /// Classic RelayCommand implementation for the MVVM pattern.
    /// </summary>
    public class RelayCommand : ICommand {

        private readonly Action _executeAction;

        /// <summary>
        /// Event for the CanExecute feature.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Create a new instance that can always execute.
        /// </summary>
        /// <param name="execute">the action to execute</param>
        /// <exception cref="T:System.ArgumentNullException">If the execute argument is null.</exception>
        public RelayCommand(Action execute) {
            if (execute == null) {
                throw new ArgumentNullException("execute");
            }
            this._executeAction = execute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">This parameter will always be ignored.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter) {
            return true;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked. 
        /// </summary>
        /// <param name="parameter">This parameter will always be ignored.</param>
        public void Execute(object parameter) {
            this._executeAction.Invoke();
        }

    }

}
