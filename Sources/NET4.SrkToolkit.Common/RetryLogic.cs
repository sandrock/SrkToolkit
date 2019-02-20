// 
// Copyright 2014 SandRock, pyDez
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

namespace SrkToolkit
{
    using SrkToolkit.Internals;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    // TODO: support for netstandard

    public static class RetryLogic
    {
        static public RetryLogicState<TReturn> Do<TReturn>(Func<TReturn> action)
        {
            return new RetryLogicState<TReturn>(action);
        }

        static public RetryLogicState<Nothing> Do(Action action)
        {
            return new RetryLogicState<Nothing>(action);
        }
#if NET45
        static public RetryLogicState<TReturn> DoAsync<TReturn>(Func<Task<TReturn>> action)
        {
            return new RetryLogicState<TReturn>(action);
        }

        static public RetryLogicState<Nothing> DoAsync(Func<Task> action)
        {
            return new RetryLogicState<Nothing>(action);
        }
#endif

        static public RetryLogicState<Nothing> BeginPrepare()
        {
            return RetryLogicState<Nothing>.BeginPrepare();
        }
    }
}
