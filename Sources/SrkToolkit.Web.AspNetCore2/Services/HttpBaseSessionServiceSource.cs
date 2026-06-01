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
    using System.Text.Json;

    // This file is excluded from the netstandard2.0 build (see csproj) because ISession and
    // System.Text.Json are only available in ASP.NET Core runtimes (net8.0+). Consumers on
    // bare netstandard2.0 cannot use this class.

    /// <summary>
    /// Default implementation of <see cref="ISessionServiceSource"/> using <see cref="ISession"/> as provider.
    /// Values are serialized to JSON strings via <see cref="JsonSerializer"/> because <see cref="ISession"/>
    /// only stores byte arrays — unlike the MVC5 <c>HttpSessionStateBase</c> which stored raw CLR objects.
    /// <see cref="BaseSessionService"/> deserializes on read.
    /// </summary>
    public class HttpBaseSessionServiceSource : ISessionServiceSource
    {
        private readonly ISession source;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpBaseSessionServiceSource"/> class.
        /// </summary>
        /// <param name="source">The ASP.NET Core session.</param>
        public HttpBaseSessionServiceSource(ISession source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
        }

        /// <summary>
        /// Gets an object by its key. Returns a JSON string; callers must deserialize to the target type.
        /// </summary>
        public object Get(string key)
        {
            return this.source.GetString(key);
        }

        /// <summary>
        /// Clears an object from session.
        /// </summary>
        public void Clear(string key)
        {
            this.source.Remove(key);
        }

        /// <summary>
        /// Serializes <paramref name="value"/> to JSON and stores it in session.
        /// </summary>
        public void Set(string key, object value)
        {
            if (value == null)
            {
                this.source.Remove(key);
                return;
            }

            this.source.SetString(key, JsonSerializer.Serialize(value));
        }

        /// <summary>
        /// Clears the session.
        /// </summary>
        public void Clear()
        {
            this.source.Clear();
        }
    }
}
