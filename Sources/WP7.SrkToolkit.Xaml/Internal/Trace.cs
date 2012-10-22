
namespace SrkToolkit
{
    using System;
    using System.Diagnostics;

    internal static class Trace
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
#if !SILVERLIGHT
        [Conditional("TRACE")]
#endif
        public static void TraceInfo(string message)
        {
#if SILVERLIGHT
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
#if !SILVERLIGHT
        [Conditional("TRACE")]
#endif
        public static void TraceInfo(string message, Exception ex)
        {
            if (ex == null)
            {
                TraceInfo(message);
            }
            else
            {
#if SILVERLIGHT
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
#if !SILVERLIGHT
        [Conditional("TRACE")]
#endif
        public static void TraceInfo(string objectName, string message)
        {
#if SILVERLIGHT
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
#if !SILVERLIGHT
        [Conditional("TRACE")]
#endif
        public static void TraceInfo(string objectName, string message, Exception ex)
        {
            if (ex == null)
                TraceInfo(objectName, message);
            else
            {
#if SILVERLIGHT
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
#if !SILVERLIGHT
        [Conditional("TRACE")]
#endif
        public static void TraceWarning(string message)
        {
#if SILVERLIGHT
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
#if !SILVERLIGHT
        [Conditional("TRACE")]
#endif
        public static void TraceWarning(string message, Exception ex)
        {
            if (ex == null)
                TraceWarning(message);
            else
            {
#if SILVERLIGHT
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
#if !SILVERLIGHT
        [Conditional("TRACE")]
#endif
        public static void TraceWarning(string objectName, string message)
        {
#if SILVERLIGHT
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
#if !SILVERLIGHT
        [Conditional("TRACE")]
#endif
        public static void TraceWarning(string objectName, string message, Exception ex)
        {
            if (ex == null)
                TraceWarning(objectName, message);
            else
            {
#if SILVERLIGHT
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
#if !SILVERLIGHT
        [Conditional("TRACE")]
#endif
        public static void TraceError(string message)
        {
#if SILVERLIGHT
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
#if !SILVERLIGHT
        [Conditional("TRACE")]
#endif
        public static void TraceError(string message, Exception ex)
        {
            if (ex == null)
                TraceError(message);
            else
            {
#if SILVERLIGHT
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
#if !SILVERLIGHT
        [Conditional("TRACE")]
#endif
        public static void TraceError(string objectName, string message)
        {
#if SILVERLIGHT
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
#if !SILVERLIGHT
        [Conditional("TRACE")]
#endif
        public static void TraceError(string objectName, string message, Exception ex)
        {
            if (ex == null)
                TraceError(objectName, message);
            else
            {
#if SILVERLIGHT
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

#if !SILVERLIGHT
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
