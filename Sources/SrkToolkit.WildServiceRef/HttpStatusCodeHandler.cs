using System;
using System.Collections.Generic;
using System.Net;

namespace SrkToolkit.WildServiceRef {

    /// <summary>
    /// Default handler for HTTP status codes.
    /// It has default behaviors but the can be appended or overriden.
    /// </summary>
    public class HttpStatusCodeHandler : IHttpStatusCodeHandler {

        private readonly List<Predicate<int>> behaviors = new List<Predicate<int>>();

        /// <summary>
        /// Default .ctor.
        /// Default behaviors will be created.
        /// </summary>
        public HttpStatusCodeHandler() {
            CreateDefaultBehaviors();
        }

        /// <summary>
        /// This method creates default behavior for most HTTP codes.
        /// </summary>
        protected void CreateDefaultBehaviors() {

            //
            // Specific codes
            //

            // 200: ok
            behaviors.Add(i => i == 200);
            // 304: not modified
            behaviors.Add(i => i == 304);

            // 403: not authorized
            behaviors.Add(i => {
                if (i == 403) {
                    throw new UnauthorizedOperationException(
                        "Service refused to serve the request because you are not authorized. ");
                }
                return false;
            });

            // 418: 
            behaviors.Add(i => {
                if (i == 418) {
                    throw new InvalidOperationException(
                        "Service made a joke (HTTP code: " + i +
                        "). You might want to try again. ");
                }
                return false;
            });

            //
            // Main codes
            //

            // 300 (redirections)
            behaviors.Add(i => {
                if (i >= 300 && i < 400) {
                    throw new WebException("Service did not respond correctly (redirection) (HTTP code: " + i + "). ");
                }
                return false;
            });

            // 400 (bad requests)
            behaviors.Add(i => {
                if (i >= 400 && i < 500 || i == 505) {
                    throw new InvalidOperationException(
                        "Service returned an error. The cause seems to be a bad request. " +
                        "Update your application or contact support. ");
                }
                return false;
            });

            // 500: server error
            behaviors.Add(i => {
                if (i >= 500 && i < 600) {
                    throw new UnavailableServiceException(
                        "Service seems to be unavailable (maintenance?), please try again later " +
                        "(HTTP code: " + i + "). ");
                }
                return false;
            });
        }

        #region IHttpStatusCodeHandler Members

        /// <summary>
        /// Handle an HTTP status code.
        /// </summary>
        /// <param name="httpStatusCode">status code to handle</param>
        public void Handle(int httpStatusCode) {
            if (behaviors.Count == 0)
                throw new ApplicationException("No behaviors are defined to handle HTTP codes.");

            foreach (var behavior in behaviors) {
                if (behavior(httpStatusCode))
                    break;
            }

            if (behaviors.Count == 0)
                throw new InvalidOperationException("HTTP code (" + httpStatusCode + ") was not handled.");
        }

        #endregion

    }
}
