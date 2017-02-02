// 
// Copyright 2014 SandRock
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

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
