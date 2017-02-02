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
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementation of <see cref="ISessionServiceSource"/> based on a simple dictionary.
    /// </summary>
    public class DictionarySessionServiceSource : ISessionServiceSource
    {
        private readonly IDictionary<string, object>  source;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionarySessionServiceSource"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public DictionarySessionServiceSource(IDictionary<string, object> source)
        {
            this.source = source;
        }

        /// <summary>
        /// Gets on object by its key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (this.source == null)
                return null;

            return this.source[key];
        }

        /// <summary>
        /// Clears an object from session.
        /// </summary>
        /// <param name="key"></param>
        public void Clear(string key)
        {
            if (this.source == null)
                return;

            this.source.Remove(key);
        }

        /// <summary>
        /// Set an object in session.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            if (this.source == null)
                return;

            this.source[key] = value;
        }

        /// <summary>
        /// Clears the session.
        /// </summary>
        public void Clear()
        {
            if (this.source == null)
                return;

            this.source.Clear();
        }
    }
}
