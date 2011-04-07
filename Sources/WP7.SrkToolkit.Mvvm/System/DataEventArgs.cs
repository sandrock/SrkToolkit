using System;

namespace System {

    /// <summary>
    ///  Generic arguments class to pass to event handlers that need to receive data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataEventArgs<T> : EventArgs {

        /// <summary>
        /// Initializes the DataEventArgs class.
        /// </summary>
        /// <param name="data"></param>
        public DataEventArgs(T data) : base() {
            Data = data;
        }

        /// <summary>
        /// Initializes the DataEventArgs class.
        /// </summary>
        public DataEventArgs() : base() {

        }

        /// <summary>
        /// Gets the information related to the event.
        /// </summary>
        public T Data { get; private set; }

    }
}
