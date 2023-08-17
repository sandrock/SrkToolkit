﻿// 
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

namespace System.Diagnostics
{
    /// <summary>
    /// Extension methods for the <see cref="Stopwatch"/> class.
    /// </summary>
    public static class StopwatchExtensions
    {
        /// <summary>
        /// Restarts and returns the elapsed time.
        /// </summary>
        /// <param name="watch"></param>
        /// <returns></returns>
        public static TimeSpan GetElapsedAndRestart(this Stopwatch watch)
        {
            var elapsed = watch.Elapsed;
            watch.Restart();
            return elapsed;
        }
    }
}
