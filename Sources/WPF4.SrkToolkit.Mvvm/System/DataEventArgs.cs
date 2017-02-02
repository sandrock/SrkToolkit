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

namespace System
{
    using System;

    /// <summary>
    ///  Generic arguments class to pass to event handlers that need to receive data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Initializes the DataEventArgs class.
        /// </summary>
        /// <param name="data"></param>
        public DataEventArgs(T data)
            : base()
        {
            Data = data;
        }

        /// <summary>
        /// Initializes the DataEventArgs class.
        /// </summary>
        public DataEventArgs()
            : base()
        {

        }

        /// <summary>
        /// Gets the information related to the event.
        /// </summary>
        public T Data { get; private set; }
    }
}
