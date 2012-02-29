
namespace System.Diagnostics
{
    /// <summary>
    /// Extended methods for the <see cref="Debug."/>
    /// </summary>
    public class TraceEx
    {
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
        }

        [Conditional("TRACE")]
        public static void Info(string message, Exception ex)
        {
            if (ex == null)
                Info(message);
            else
                Debug.WriteLine("Information: " + Time + " " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
        }

        [Conditional("TRACE")]
        public static void Info(string objectName, string message)
        {
            Debug.WriteLine("Information: " + Time + " " + objectName + ": " + message);
        }

        [Conditional("TRACE")]
        public static void Info(string objectName, string message, Exception ex)
        {
            if (ex == null)
                Info(objectName, message);
            else
                Debug.WriteLine("Information: " + Time + " " + objectName + ": " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message);
        }

        //[Conditional("TRACE")]
        //public static void Info(object inObject, string message)
        //{
        //    Debug.WriteLine("Information: " + Time + " " + inObject.GetType().Name + ": " + message);
        //}

        //[Conditional("TRACE")]
        //public static void Info(object inObject, string message, Exception ex)
        //{
        //    if (ex == null)
        //        Info(inObject, message);
        //    else
        //        Debug.WriteLine("Information: " + Time + " " + inObject.GetType().Name + ": " + message +
        //            Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
        //            Environment.NewLine + ex.StackTrace);
        //}

        #endregion

        #region Warn

        [Conditional("TRACE")]
        public static void Warning(string message)
        {
            Debug.WriteLine("Warning: " + Time + " " + message);
        }

        [Conditional("TRACE")]
        public static void Warning(string message, Exception ex)
        {
            if (ex == null)
                Warning(message);
            else
                Debug.WriteLine("Warning: " + Time + " " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
        }

        [Conditional("TRACE")]
        public static void Warning(string objectName, string message)
        {
            Debug.WriteLine("Warning: " + Time + " " + objectName + ": " + message);
        }

        [Conditional("TRACE")]
        public static void Warning(string objectName, string message, Exception ex)
        {
            if (ex == null)
                Warning(objectName, message);
            else
                Debug.WriteLine("Warning: " + Time + " " + objectName + ": " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message);
        }

        //[Conditional("TRACE")]
        //public static void Warning(object inObject, string message)
        //{
        //    Debug.WriteLine("Warning: " + Time + " " + inObject.GetType().Name + ": " + message);
        //}

        //[Conditional("TRACE")]
        //public static void Warning(object inObject, string message, Exception ex)
        //{
        //    if (ex == null)
        //        Warning(inObject, message);
        //    else
        //        Debug.WriteLine("Warning: " + Time + " " + inObject.GetType().Name + ": " + message +
        //            Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
        //            Environment.NewLine + ex.StackTrace);
        //}

        #endregion

        #region Error

        [Conditional("TRACE")]
        public static void Error(string message)
        {
            Debug.WriteLine("Error: " + Time + " " + message);
        }

        [Conditional("TRACE")]
        public static void Error(string message, Exception ex)
        {
            if (ex == null)
                Error(message);
            else
                Debug.WriteLine("Error: " + Time + " " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
        }

        //[Conditional("TRACE")]
        //public static void Error(object inObject, string message)
        //{
        //    Debug.WriteLine("Error: " + Time + " " + inObject.GetType().Name + ": " + message);
        //}

        //[Conditional("TRACE")]
        //public static void Error(object inObject, string message, Exception ex)
        //{
        //    if (ex == null)
        //        Error(inObject, message);
        //    else
        //        Debug.WriteLine("Error: " + Time + " " + inObject.GetType().Name + ": " + message +
        //            Environment.NewLine + ex.GetType().Name + ": " + ex.Message);
        //}

        [Conditional("TRACE")]
        public static void Error(string objectName, string message)
        {
            Debug.WriteLine("Error: " + Time + " " + objectName + ": " + message);
        }

        [Conditional("TRACE")]
        public static void Error(string objectName, string message, Exception ex)
        {
            if (ex == null)
                Error(objectName, message);
            else
                Debug.WriteLine("Error: " + Time + " " + objectName + ": " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
        }

        #endregion
    }
}
