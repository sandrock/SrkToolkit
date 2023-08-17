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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// THe precision of a <see cref="DateTime" />.
    /// </summary>
    public enum DateTimePrecision
    {
        /// <summary>
        /// The year
        /// </summary>
        Year,

        /// <summary>
        /// The month
        /// </summary>
        Month,

        /// <summary>
        /// The day
        /// </summary>
        Day,

        /// <summary>
        /// The hour
        /// </summary>
        Hour,

        /// <summary>
        /// The minute
        /// </summary>
        Minute,

        /// <summary>
        /// The second
        /// </summary>
        Second,

        /// <summary>
        /// The millisecond
        /// </summary>
        Millisecond,
    }
}
