using System;
using System.Collections.Generic;

namespace SrkToolkit.WcfServiceRef.Common {

    /// <summary>
    /// Represent an API response with no data.
    /// </summary>
    public class AsyncResponseArgs : EventArgs {

        // <summary>
        // Class .ctor to mark the response as a unknown failure.
        // </summary>
        //public AsyncResponseArgs() {
        //
        //}

        /// <summary>
        /// Class .ctor to mark the response as a success with no data.
        /// </summary>
        /// <param name="succeed"></param>
        public AsyncResponseArgs() : this(true, null) { }

        /// <summary>
        /// Class .ctor to mark the response as a success or failure with no data.
        /// </summary>
        /// <param name="succeed"></param>
        public AsyncResponseArgs(bool succeed) : this(succeed, null) { }

        /// <summary>
        /// Class .ctor to mark the response as a failure with no data.
        /// </summary>
        /// <param name="exception"></param>
        public AsyncResponseArgs(Exception exception) : this(exception, null) { }

        /// <summary>
        /// Class .ctor to mark the response as a success or failure with no data.
        /// </summary>
        /// <param name="succeed"></param>
        /// <param name="requestData"></param>
        public AsyncResponseArgs(bool succeed, Dictionary<string, string> requestData) {
            this.Succeed = succeed;
            this.requestData = requestData;
        }

        /// <summary>
        /// lass .ctor to mark the response as a failure with no data.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="requestData"></param>
        public AsyncResponseArgs(Exception exception, Dictionary<string, string> requestData) {
            this.Error = exception;
            this.Succeed = false;
            this.requestData = requestData;
        }

        /// <summary>
        /// Informs of a successful request.
        /// </summary>
        public bool Succeed { get; set; }

        /// <summary>
        /// Internal error.
        /// </summary>
        public Exception Error { get; set; }

        /// <summary>
        /// Contains data from the initial request.
        /// This field is not used yet.
        /// </summary>
        public Dictionary<string, string> RequestData { get { return requestData; } }
        private readonly Dictionary<string, string> requestData;

        /// <summary>
        /// Returns an error message from <see cref="Error"/>.
        /// </summary>
        public string ErrorMessage {
            get {
                return Error != null ? Error.Message : null;
            }
        }

    }

    /// <summary>
    /// Represent an API response with data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncResponseArgs<T> : AsyncResponseArgs {

        /// <summary>
        /// Class .ctor to mark the response as a failure with no data.
        /// </summary>
        /// <param name="exception"></param>
        public AsyncResponseArgs(Exception exception) : base(exception) { }

        /// <summary>
        /// Class .ctor to mark the response as a success with data.
        /// </summary>
        /// <param name="data"></param>
        public AsyncResponseArgs(T data) : this(data, null) { }

        /// <summary>
        /// Class .ctor to mark the response as a failure with no data.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="requestData"></param>
        public AsyncResponseArgs(Exception exception, Dictionary<string, string> requestData) : base(exception, requestData) { }

        /// <summary>
        /// Class .ctor to mark the response as a success with data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="requestData"></param>
        public AsyncResponseArgs(T data, Dictionary<string, string> requestData)
            : base(true, requestData) {
            this.Data = data;
        }

        /// <summary>
        /// Attached data from the service.
        /// This can be null event if <see cref="AsyncResponseArgs.Succeed"/> is true.
        /// </summary>
        public T Data { get; set; }

    }

}
