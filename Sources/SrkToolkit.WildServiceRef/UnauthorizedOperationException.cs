using System;

namespace SrkToolkit.WildServiceRef {

    /// <summary>
    /// The exception that is thrown when a service call resulted in a denial to respond. 
    /// Meaning the current user is not allowed to perform an operation.
    /// </summary>
    [Serializable]
    public class UnauthorizedOperationException : Exception {

        /// <summary>
        /// Initialize a new instance of <see cref="UnauthorizedOperationException"/> class.
        /// </summary>
        public UnauthorizedOperationException() { }

        /// <summary>
        /// Initialize a new instance of <see cref="UnauthorizedOperationException"/> class 
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UnauthorizedOperationException(string message) : base(message) { }

        /// <summary>
        /// Initialize a new instance of <see cref="UnauthorizedOperationException"/> class 
        /// with a specified error message and a reference to the inner exception that is 
        /// the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public UnauthorizedOperationException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initialize a new instance of <see cref="UnauthorizedOperationException"/> class 
        /// with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected UnauthorizedOperationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
