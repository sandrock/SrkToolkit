
namespace SrkToolkit.Mvvm.Tools {

    /// <summary>
    /// Represents the state of a task.
    /// </summary>
    public enum BusyTaskType {

        /// <summary>
        /// Normal state.
        /// </summary>
        Default,

        /// <summary>
        /// The operation was successful.
        /// </summary>
        Confirmation,

        /// <summary>
        /// The operation failed.
        /// </summary>
        Error,

        /// <summary>
        /// The operation encoutered an issue.
        /// </summary>
        Warning,
    }
}
