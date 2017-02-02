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

namespace SrkToolkit.Domain
{
    using System;

    /// <summary>
    /// Represents some kind of domain error.
    /// </summary>
    public interface IResultError
    {
        /// <summary>
        /// Gets the error display message.
        /// </summary>
        string DisplayMessage { get; }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Gets or sets an additional information.
        /// </summary>
        string Detail { get; }
    }
}
