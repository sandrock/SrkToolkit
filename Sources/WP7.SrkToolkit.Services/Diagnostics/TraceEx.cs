
namespace System.Diagnostics
{
    /// <summary>
    /// Extended methods for the <see cref="Debug."/>
    /// </summary>
    public partial class TraceEx
    {
        public static EventHandler<TraceEventArgs> NewEvent;

        private static string Time
        {
            get
            {
                return DateTime.Now.ToString();
            }
        }

        #region Info

        [Conditional("TRACE")]
        public static void Info(string message)
        {
            Debug.WriteLine("Information: " + Time + " " + message);
            RaiseEvent("Info", null, message, null);
        }

        [Conditional("TRACE")]
        public static void Info(string message, Exception ex)
        {
            if (ex != null)
            {
                Debug.WriteLine("Information: " + Time + " " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
                RaiseEvent("Info", null, message, ex);
            }
            else
                Info(message);
        }

        [Conditional("TRACE")]
        public static void Info(string objectName, string message)
        {
            Debug.WriteLine("Information: " + Time + " " + objectName + ": " + message);
            RaiseEvent("Info", objectName, message, null);
        }

        [Conditional("TRACE")]
        public static void Info(string objectName, string message, Exception ex)
        {
            if (ex == null)
                Info(objectName, message);
            else
            {
                Debug.WriteLine("Information: " + Time + " " + objectName + ": " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message);
                RaiseEvent("Info", objectName, message, ex);
            }
        }

        #endregion

        #region Warn

        [Conditional("TRACE")]
        public static void Warning(string message)
        {
            Debug.WriteLine("Warning: " + Time + " " + message);
            RaiseEvent("Warning", null, message, null);
        }

        [Conditional("TRACE")]
        public static void Warning(string message, Exception ex)
        {
            if (ex == null)
                Warning(message);
            else
            {
                Debug.WriteLine("Warning: " + Time + " " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
                RaiseEvent("Warning", null, message, ex);
            }
        }

        [Conditional("TRACE")]
        public static void Warning(string objectName, string message)
        {
            Debug.WriteLine("Warning: " + Time + " " + objectName + ": " + message);
            RaiseEvent("Warning", objectName, message, null);
        }

        [Conditional("TRACE")]
        public static void Warning(string objectName, string message, Exception ex)
        {
            if (ex == null)
                Warning(objectName, message);
            else
            {
                Debug.WriteLine("Warning: " + Time + " " + objectName + ": " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message);
                RaiseEvent("Warning", objectName, message, ex);
            }
        }

        #endregion

        #region Error

        [Conditional("TRACE")]
        public static void Error(string message)
        {
            Debug.WriteLine("Error: " + Time + " " + message);
            RaiseEvent("Error", null, message, null);
        }

        [Conditional("TRACE")]
        public static void Error(string message, Exception ex)
        {
            if (ex == null)
                Error(message);
            else
            {
                Debug.WriteLine("Error: " + Time + " " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
                RaiseEvent("Error", null, message, ex);
            }
        }

        [Conditional("TRACE")]
        public static void Error(string objectName, string message)
        {
            Debug.WriteLine("Error: " + Time + " " + objectName + ": " + message);
            RaiseEvent("Error", objectName, message, null);
        }

        [Conditional("TRACE")]
        public static void Error(string objectName, string message, Exception ex)
        {
            if (ex == null)
                Error(objectName, message);
            else
            {
                Debug.WriteLine("Error: " + Time + " " + objectName + ": " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
                RaiseEvent("Error", objectName, message, ex);
            }
        }

        #endregion
        private static void RaiseEvent(string kind, string objectName, string message, Exception exception)
        {
            var handler = NewEvent;
            if (handler != null)
                handler(null, new TraceEventArgs
                {
                    ObjectName = objectName,
                    Message = message,
                    Exception = exception,
                    Kind = kind,
                });
        }
    }

    public class TraceEventArgs : EventArgs
    {
        public string Kind { get; set; }
        public string ObjectName { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
