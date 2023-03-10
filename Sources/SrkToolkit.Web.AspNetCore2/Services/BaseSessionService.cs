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

    /// <summary>
    /// 
    /// </summary>
    public class BaseSessionService
    {
        private readonly ISessionServiceSource source;

        /// <summary>
        /// Initializes an instance with a HttpSessionStateBase object (likely from ASP MVC).
        /// </summary>
        /// <param name="httpSessionStateBase"></param>
        public BaseSessionService(HttpSessionStateBase httpSessionStateBase)
        {
            this.source = new HttpBaseSessionServiceSource(httpSessionStateBase);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSessionService"/> class.
        /// </summary>
        /// <param name="httpSessionDictionary">The HTTP session dictionary.</param>
        public BaseSessionService(IDictionary<string, object> httpSessionDictionary)
        {
            this.source = new DictionarySessionServiceSource(httpSessionDictionary);
        }

        /// <summary>
        /// Clears everything form the session.
        /// </summary>
        public void Clear()
        {
            this.source.Clear();
        }

        /// <summary>
        /// Clears a session value.
        /// </summary>
        /// <param name="key"></param>
        protected void Clear(string key)
        {
            this.source.Clear(key);
        }

        /// <summary>
        /// Sets a session value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void Set<T>(string key, T value)
        {
            this.source.Set(key, value);
        }


        /// <summary>
        /// Gets a session reference-type value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected T GetObject<T>(string key)
            where T : class
        {
            var obj = this.source.Get(key);
            if (obj == null)
                return null;

            return (T)obj;
        }

        /// <summary>
        /// Gets a session nullable value-type value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected T? GetValue<T>(string key)
            where T : struct
        {
            var obj = this.source.Get(key);
            if (obj == null)
                return default(T?);

            return (T)obj;
        }
    }
}
