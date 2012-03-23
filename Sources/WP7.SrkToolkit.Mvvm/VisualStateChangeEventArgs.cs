using System;

namespace SrkToolkit.Mvvm {
    /// <summary>
    /// Used to command a visual state transition.
    /// </summary>
    public class VisualStateChangeEventArgs : EventArgs {
        private string stateName;
        private bool useTransitions;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualStateChangeEventArgs"/> class.
        /// </summary>
        /// <param name="stateName">Name of the state to reach.</param>
        /// <param name="useTransitions">if set to <c>true</c> [use transitions].</param>
        public VisualStateChangeEventArgs(string stateName, bool useTransitions) {
            this.stateName = stateName;
            this.useTransitions = useTransitions;
        }

        /// <summary>
        /// Gets the name of the state to reach.
        /// </summary>
        /// <value>
        /// The name of the state to reach.
        /// </value>
        public string StateName {
            get { return this.stateName; }
        }

        /// <summary>
        /// Gets a value indicating whether [use transitions].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use transitions]; otherwise, <c>false</c>.
        /// </value>
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
    }
}
