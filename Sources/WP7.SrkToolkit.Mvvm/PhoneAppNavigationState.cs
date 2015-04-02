
namespace SrkToolkit.Mvvm
{
    /// <summary>
    /// Navigation states for a Windows Phone 7 application.
    /// </summary>
    public enum PhoneAppNavigationState
    {
        /// <summary>
        /// Before the first navigation of the application lifetime (ephemeral state).
        /// </summary>
        Starting,

        /// <summary>
        /// First navigation of the application lifetime.
        /// </summary>
        Started,

        /// <summary>
        /// Internal navigation.
        /// </summary>
        Internal,

        /// <summary>
        /// Before the application is activated (ephemeral state).
        /// </summary>
        Activated,

        /// <summary>
        /// Application is activated.
        /// </summary>
        Activating,

        /// <summary>
        /// The application is deactivated.
        /// </summary>
        Deactivated,

        /// <summary>
        /// The application is closed.
        /// </summary>
        Closed,
    }
}
