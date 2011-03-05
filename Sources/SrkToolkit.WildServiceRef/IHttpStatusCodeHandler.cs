
namespace SrkToolkit.WildServiceRef {

    /// <summary>
    /// Designed for HTTP status code handling.
    /// </summary>
    public interface IHttpStatusCodeHandler {

        /// <summary>
        /// Handle an HTTP status code.
        /// </summary>
        /// <param name="httpStatusCode">status code to handle</param>
        void Handle(int httpStatusCode);

    }
}
