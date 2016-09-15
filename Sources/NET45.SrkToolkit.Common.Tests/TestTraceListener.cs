
namespace SrkToolkit.Common.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public class TestTraceListener : TraceListener
    {
        public event EventHandler<TraceEventArgs> TraceWrite;

        public override void Write(string message)
        {
            var handler = this.TraceWrite;
            if (handler != null)
                handler(this, new TraceEventArgs(message));
        
        }

        public override void WriteLine(string message)
        {
            var handler = this.TraceWrite;
            if (handler != null)
                handler(this, new TraceEventArgs(message));
        
        }

        public class TraceEventArgs : EventArgs
        {
            public TraceEventArgs(string message)
            {
                this.Message = message;
            }

            public string Message { get; set; }
        }
    }
}
