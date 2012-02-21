
namespace System.Diagnostics
{
    /// <summary>
    /// Extended methods for the <see cref="Trace."/>
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
            Trace.TraceInformation(Time + " " + message);
        }

        [Conditional("TRACE")]
        public static void Info(string message, Exception ex)
        {
            if (ex == null)
                Info(message);
            else
                Trace.TraceInformation(Time + " " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
        }

        [Conditional("TRACE")]
        public static void Info(string objectName, string message)
        {
            Trace.TraceInformation(Time + " " + objectName + ": " + message);
        }

        [Conditional("TRACE")]
        public static void Info(string objectName, string message, Exception ex)
        {
            if (ex == null)
                Info(objectName, message);
            else
                Trace.TraceInformation(Time + " " + objectName + ": " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message);
        }

        //[Conditional("TRACE")]
        //public static void Info(object inObject, string message)
        //{
        //    Trace.TraceInformation(Time + " " + inObject.GetType().Name + ": " + message);
        //}

        //[Conditional("TRACE")]
        //public static void Info(object inObject, string message, Exception ex)
        //{
        //    if (ex == null)
        //        Info(inObject, message);
        //    else
        //        Trace.TraceInformation(Time + " " + inObject.GetType().Name + ": " + message +
        //            Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
        //            Environment.NewLine + ex.StackTrace);
        //}

        #endregion

        #region Warn

        [Conditional("TRACE")]
        public static void Warning(string message)
        {
            Trace.TraceWarning(Time + " " + message);
        }

        [Conditional("TRACE")]
        public static void Warning(string message, Exception ex)
        {
            if (ex == null)
                Warning(message);
            else
                Trace.TraceWarning(Time + " " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
        }

        [Conditional("TRACE")]
        public static void Warning(string objectName, string message)
        {
            Trace.TraceWarning(Time + " " + objectName + ": " + message);
        }

        [Conditional("TRACE")]
        public static void Warning(string objectName, string message, Exception ex)
        {
            if (ex == null)
                Warning(objectName, message);
            else
                Trace.TraceWarning(Time + " " + objectName + ": " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message);
        }

        //[Conditional("TRACE")]
        //public static void Warning(object inObject, string message)
        //{
        //    Trace.TraceWarning(Time + " " + inObject.GetType().Name + ": " + message);
        //}

        //[Conditional("TRACE")]
        //public static void Warning(object inObject, string message, Exception ex)
        //{
        //    if (ex == null)
        //        Warning(inObject, message);
        //    else
        //        Trace.TraceWarning(Time + " " + inObject.GetType().Name + ": " + message +
        //            Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
        //            Environment.NewLine + ex.StackTrace);
        //}

        #endregion

        #region Error

        [Conditional("TRACE")]
        public static void Error(string message)
        {
            Trace.TraceError(Time + " " + message);
        }

        [Conditional("TRACE")]
        public static void Error(string message, Exception ex)
        {
            if (ex == null)
                Error(message);
            else
                Trace.TraceError(Time + " " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
        }

        //[Conditional("TRACE")]
        //public static void Error(object inObject, string message)
        //{
        //    Trace.TraceError(Time + " " + inObject.GetType().Name + ": " + message);
        //}

        //[Conditional("TRACE")]
        //public static void Error(object inObject, string message, Exception ex)
        //{
        //    if (ex == null)
        //        Error(inObject, message);
        //    else
        //        Trace.TraceError(Time + " " + inObject.GetType().Name + ": " + message +
        //            Environment.NewLine + ex.GetType().Name + ": " + ex.Message);
        //}

        [Conditional("TRACE")]
        public static void Error(string objectName, string message)
        {
            Trace.TraceError(Time + " " + objectName + ": " + message);
        }

        [Conditional("TRACE")]
        public static void Error(string objectName, string message, Exception ex)
        {
            if (ex == null)
                Error(objectName, message);
            else
                Trace.TraceError(Time + " " + objectName + ": " + message +
                    Environment.NewLine + ex.GetType().Name + ": " + ex.Message +
                    Environment.NewLine + ex.StackTrace);
        }

        #endregion

        [Conditional("TRACE")]
        public static void Flush()
        {
            Trace.Flush();
        }
    }
}
