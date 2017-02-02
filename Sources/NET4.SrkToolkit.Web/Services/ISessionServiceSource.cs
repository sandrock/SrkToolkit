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

namespace SrkToolkit.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Text;

    /// <summary>
    /// Abstraction of <see cref="HttpSessionStateBase"/>.
    /// </summary>
    public interface ISessionServiceSource
    {
        /// <summary>
        /// Gets on object by its key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// Clears an object from session.
        /// </summary>
        /// <param name="key"></param>
        void Clear(string key);

        /// <summary>
        /// Set an object in session.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(string key, object value);

        /// <summary>
        /// Clears the session.
        /// </summary>
        void Clear();
    }
}
