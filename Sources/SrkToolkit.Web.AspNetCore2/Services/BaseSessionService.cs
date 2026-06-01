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
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;

    // This file is excluded from the netstandard2.0 build (see csproj) — same reason as
    // HttpBaseSessionServiceSource: depends on ISession and System.Text.Json.

    /// <summary>
    /// Base class for session services. Inherit and expose typed properties backed by <see cref="ISession"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="GetObject{T}"/> and <see cref="GetValue{T}"/> handle both direct object storage
    /// (used by <see cref="DictionarySessionServiceSource"/> in tests) and JSON strings
    /// (used by <see cref="HttpBaseSessionServiceSource"/> at runtime).
    /// </remarks>
    public class BaseSessionService
    {
        private readonly ISessionServiceSource source;

        /// <summary>
        /// Initializes a new instance using an ASP.NET Core <see cref="ISession"/>.
        /// </summary>
        public BaseSessionService(ISession session)
        {
            this.source = new HttpBaseSessionServiceSource(session ?? throw new ArgumentNullException(nameof(session)));
        }

        /// <summary>
        /// Initializes a new instance using a plain dictionary (useful for unit tests).
        /// </summary>
        public BaseSessionService(IDictionary<string, object> httpSessionDictionary)
        {
            this.source = new DictionarySessionServiceSource(httpSessionDictionary);
        }

        /// <summary>
        /// Clears everything from the session.
        /// </summary>
        public void Clear()
        {
            this.source.Clear();
        }

        /// <summary>
        /// Clears a session value.
        /// </summary>
        protected void Clear(string key)
        {
            this.source.Clear(key);
        }

        /// <summary>
        /// Sets a session value.
        /// </summary>
        protected void Set<T>(string key, T value)
        {
            this.source.Set(key, value);
        }

        /// <summary>
        /// Gets a session reference-type value.
        /// </summary>
        protected T GetObject<T>(string key)
            where T : class
        {
            var obj = this.source.Get(key);
            if (obj == null)
                return null;
            if (obj is T t)
                return t;
            if (obj is string json)
                return JsonSerializer.Deserialize<T>(json);
            return (T)obj;
        }

        /// <summary>
        /// Gets a session nullable value-type value.
        /// </summary>
        protected T? GetValue<T>(string key)
            where T : struct
        {
            var obj = this.source.Get(key);
            if (obj == null)
                return default(T?);
            if (obj is T t)
                return t;
            if (obj is string json)
                return JsonSerializer.Deserialize<T>(json);
            return (T)obj;
        }
    }
}
