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

namespace SrkToolkit.Mvvm.Commands
{
    using System;
    using System.Windows.Input;
    using System.Diagnostics;

    /// <summary>
    /// Classic generic RelayCommand implementation for the MVVM pattern.
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> canExecuteFunc;
        private readonly Action<T> executeAction;
        private readonly bool canExecutePreventsExecute;

#if SILVERLIGHT || WPF
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this.canExecuteFunc != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (this.canExecuteFunc != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }
#elif UWP
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;
#endif

        /// <summary>
        /// Create a new instance that can always execute.
        /// </summary>
        /// <param name="execute">the action to execute</param>
        /// <exception cref="T:System.ArgumentNullException">If the execute argument is null.</exception>
        [DebuggerStepThrough]
        public RelayCommand(Action<T> execute)
            : this(execute, null, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <param name="canExecutePreventsExecute">if set to <c>true</c> the canExecute predicate prevents execution.</param>
        /// <exception cref="T:System.ArgumentNullException">If the execute argument is null.</exception>
        [DebuggerStepThrough]
        public RelayCommand(Action<T> execute, Predicate<T> canExecute, bool canExecutePreventsExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this.executeAction = execute;
            this.canExecuteFunc = canExecute;
            this.canExecutePreventsExecute = canExecutePreventsExecute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">This parameter will always be ignored.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return ((this.canExecuteFunc == null) ? true : this.canExecuteFunc.Invoke((T)parameter));
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked. 
        /// </summary>
        /// <param name="parameter">This parameter will always be ignored.</param>
        [DebuggerStepThrough]
        public void Execute(object parameter)
        {
            if (this.canExecutePreventsExecute && this.canExecuteFunc != null)
            {
                if (this.canExecuteFunc((T)parameter))
                    this.executeAction.Invoke((T)parameter);
            }
            else
            {
                this.executeAction.Invoke((T)parameter);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:GalaSoft.MvvmLight.Command.RelayCommand.CanExecuteChanged" /> event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
#if SILVERLIGHT || WPF
            CommandManager.InvalidateRequerySuggested();
#endif
        }
    }
}
