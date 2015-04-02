using System;
using System.Windows.Input;
using System.Diagnostics;

namespace SrkToolkit.Mvvm.Commands {

    /// <summary>
    /// Classic RelayCommand implementation for the MVVM pattern.
    /// </summary>
    public class RelayCommand : ICommand {

        private readonly Func<bool> _canExecuteFunc;
        private readonly Action _executeAction;
        private readonly bool canExecutePreventsExecute;

        /// <summary>
        /// Event for the CanExecute feature.
        /// </summary>
        public event EventHandler CanExecuteChanged {
            add {
                if (this._canExecuteFunc != null) {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove {
                if (this._canExecuteFunc != null) {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        /// Create a new instance that can always execute.
        /// </summary>
        /// <param name="execute">the action to execute</param>
        /// <exception cref="T:System.ArgumentNullException">If the execute argument is null.</exception>
        [DebuggerStepThrough]
        public RelayCommand(Action execute)
            : this(execute, null, false) {
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <param name="canExecutePreventsExecute">if set to <c>true</c> the canExecute predicate prevents execution.</param>
        /// <exception cref="T:System.ArgumentNullException">If the execute argument is null.</exception>
        [DebuggerStepThrough]
        public RelayCommand(Action execute, Func<bool> canExecute, bool canExecutePreventsExecute) {
            if (execute == null) {
                throw new ArgumentNullException("execute");
            }
            this._executeAction = execute;
            this._canExecuteFunc = canExecute;
            this.canExecutePreventsExecute = canExecutePreventsExecute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">This parameter will always be ignored.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter) {
            return ((this._canExecuteFunc == null) ? true : this._canExecuteFunc.Invoke());
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked. 
        /// </summary>
        /// <param name="parameter">This parameter will always be ignored.</param>
        [DebuggerStepThrough]
        public void Execute(object parameter) {
            if (this.canExecutePreventsExecute && this._canExecuteFunc != null) {
                if (this._canExecuteFunc())
                    this._executeAction.Invoke();
            } else {
                this._executeAction.Invoke();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:GalaSoft.MvvmLight.Command.RelayCommand.CanExecuteChanged" /> event.
        /// </summary>
        public void RaiseCanExecuteChanged() {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
