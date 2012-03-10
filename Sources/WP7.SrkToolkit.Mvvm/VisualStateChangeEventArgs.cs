using System;

namespace SrkToolkit.Mvvm {
    /// <summary>
    /// Used to command a visual state transition.
    /// </summary>
    public class VisualStateChangeEventArgs : EventArgs {
        private string stateName;
        private bool useTransitions;

        public VisualStateChangeEventArgs(string stateName, bool useTransitions) {
            this.stateName = stateName;
            this.useTransitions = useTransitions;
        }

        public string StateName {
            get { return this.stateName; }
        }

        public bool UseTransitions {
            get { return this.useTransitions; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="VisualStateChangeEventArgs"/> succeed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if operation succeed; otherwise, <c>false</c>.
        /// </value>
        public bool Succeed { get; set; }

        ////public string PreviousState { get; set; }
    }
}
