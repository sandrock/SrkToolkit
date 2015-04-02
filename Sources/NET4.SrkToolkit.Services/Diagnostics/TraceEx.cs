
namespace System.Diagnostics
{
    /// <summary>
    /// Extended methods for the <see cref="Trace"/> class.
    /// </summary>
    public class TraceEx
    {
        private static string Time
        {
            get
            {
                return DateTime.UtcNow.ToString("s");
            }
        }

        #region Info

        /// <summary>
        /// Writes an informational message.
        /// </summary>
        /// <param name="message">The message.</param>
#if !SILVERLIGHT && !NETFX_CORE
        [Conditional("TRACE")]
#endif
        public static void Info(string message)
        {
#if SILVERLIGHT || NETFX_CORE
            Debug.WriteLine("Information: " + Time + " " + message);
#else
            Trace.TraceInformation(Time + " " + message);
#endif
        }

        /// <summary>
        /// Writes an informational message with an exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception (can be null).</param>
#if !SILVERLIGHT && !NETFX_CORE
        [Conditional("TRACE")]
#endif
        public static void Info(string message, Exception ex)
        {
            if (ex == null)
            {
                Info(message);
            }
            else
            {
#if SILVERLIGHT || NETFX_CORE
                Debug.WriteLine("Information: " + Time + " " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
#else
                Trace.TraceInformation(Time + " " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
#endif
            }
        }

        /// <summary>
        /// Writes an informational message.
        /// </summary>
        /// <param name="objectName">Name of the object.</param>
        /// <param name="message">The message.</param>
#if !SILVERLIGHT && !NETFX_CORE
        [Conditional("TRACE")]
#endif
        public static void Info(string objectName, string message)
        {
#if SILVERLIGHT || NETFX_CORE
            Debug.WriteLine("Information: " + Time + " " + objectName + ": " + message);
#else
            Trace.TraceInformation(Time + " " + objectName + ": " + message);
#endif
        }

        /// <summary>
        /// Writes an informational message with an exception.
        /// </summary>
        /// <param name="objectName">Name of the object.</param>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception (can be null).</param>
#if !SILVERLIGHT && !NETFX_CORE
        [Conditional("TRACE")]
#endif
        public static void Info(string objectName, string message, Exception ex)
        {
            if (ex == null)
                Info(objectName, message);
            else
            {
#if SILVERLIGHT || NETFX_CORE
                Debug.WriteLine("Information: " + Time + " " + objectName + ": " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
#else
                Trace.TraceInformation(Time + " " + objectName + ": " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message);
#endif
            }
        }

        #endregion

        #region Warn

        /// <summary>
        /// Writes a warning message.
        /// </summary>
        /// <param name="message">The message.</param>
#if !SILVERLIGHT && !NETFX_CORE
        [Conditional("TRACE")]
#endif
        public static void Warning(string message)
        {
#if SILVERLIGHT || NETFX_CORE
            Debug.WriteLine("Warning: " + Time + " " + message);
#else
            Trace.TraceWarning(Time + " " + message);
#endif
        }

        /// <summary>
        /// Writes a warning message with an exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception (can be null).</param>
#if !SILVERLIGHT && !NETFX_CORE
        [Conditional("TRACE")]
#endif
        public static void Warning(string message, Exception ex)
        {
            if (ex == null)
                Warning(message);
            else
            {
#if SILVERLIGHT || NETFX_CORE
                Debug.WriteLine("Warning: " + Time + " " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
#else
                Trace.TraceWarning(Time + " " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
#endif
            }
        }

        /// <summary>
        /// Writes a warning message.
        /// </summary>
        /// <param name="objectName">Name of the object.</param>
        /// <param name="message">The message.</param>
#if !SILVERLIGHT && !NETFX_CORE
        [Conditional("TRACE")]
#endif
        public static void Warning(string objectName, string message)
        {
#if SILVERLIGHT || NETFX_CORE
            Debug.WriteLine("Warning: " + Time + " " + objectName + ": " + message);
#else
            Trace.TraceWarning(Time + " " + objectName + ": " + message);
#endif
        }

        /// <summary>
        /// Writes a warning message with an exception.
        /// </summary>
        /// <param name="objectName">Name of the object.</param>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception (can be null).</param>
#if !SILVERLIGHT && !NETFX_CORE
        [Conditional("TRACE")]
#endif
        public static void Warning(string objectName, string message, Exception ex)
        {
            if (ex == null)
                Warning(objectName, message);
            else
            {
#if SILVERLIGHT || NETFX_CORE
                Debug.WriteLine("Warning: " + Time + " " + objectName + ": " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
#else
                Trace.TraceWarning(Time + " " + objectName + ": " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message);
#endif
            }
        }

        #endregion

        #region Error

        /// <summary>
        /// Writes an error message.
        /// </summary>
        /// <param name="message">The message.</param>
#if !SILVERLIGHT && !NETFX_CORE
        [Conditional("TRACE")]
#endif
        public static void Error(string message)
        {
#if SILVERLIGHT || NETFX_CORE
            Debug.WriteLine("ERROR: " + Time + " " + message);
#else
            Trace.TraceError(Time + " " + message);
#endif
        }

        /// <summary>
        /// Writes an error message with an exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception (can be null).</param>
#if !SILVERLIGHT && !NETFX_CORE
        [Conditional("TRACE")]
#endif
        public static void Error(string message, Exception ex)
        {
            if (ex == null)
                Error(message);
            else
            {
#if SILVERLIGHT || NETFX_CORE
                Debug.WriteLine("ERROR: " + Time + " " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
#else
                Trace.TraceError(Time + " " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
#endif
            }
        }

        /// <summary>
        /// Writes an error message.
        /// </summary>
        /// <param name="objectName">Name of the object.</param>
        /// <param name="message">The message.</param>
#if !SILVERLIGHT && !NETFX_CORE
        [Conditional("TRACE")]
#endif
        public static void Error(string objectName, string message)
        {
#if SILVERLIGHT || NETFX_CORE
            Debug.WriteLine("ERROR: " + Time + " " + objectName + ": " + message);
#else
            Trace.TraceError(Time + " " + objectName + ": " + message);
#endif
        }

        /// <summary>
        /// Writes an error message with an exception.
        /// </summary>
        /// <param name="objectName">Name of the object.</param>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception (can be null).</param>
#if !SILVERLIGHT && !NETFX_CORE
        [Conditional("TRACE")]
#endif
        public static void Error(string objectName, string message, Exception ex)
        {
            if (ex == null)
                Error(objectName, message);
            else
            {
#if SILVERLIGHT || NETFX_CORE
                Debug.WriteLine("ERROR: " + Time + " " + objectName + ": " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
#else
                Trace.TraceError(Time + " " + objectName + ": " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message);
#endif
            }
        }

        #endregion

#if !SILVERLIGHT && !NETFX_CORE
        /// <summary>
        /// Flushes the output buffer.
        /// </summary>
        [Conditional("TRACE")]
        public static void Flush()
        {
            Trace.Flush();
        }
#endif
    }
}
