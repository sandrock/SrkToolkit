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

namespace SrkToolkit.Web.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Implementation of <see cref="HttpSessionStateBase"/> where the developer can set any property to any value.
    /// </summary>
    public class BasicHttpSessionState : HttpSessionStateBase
    {
        private Dictionary<string, object> values = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicHttpSessionState"/> class.
        /// </summary>
        public BasicHttpSessionState()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicHttpSessionState"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        public BasicHttpSessionState(Dictionary<string, object> values)
        {
            this.values = values;
        }

        /// <summary>
        /// Adds an item to the session-state collection.
        /// </summary>
        /// <param name="name">The name of the item to add to the session-state collection.</param>
        /// <param name="value">The value of the item to add to the session-state collection.</param>
        public override void Add(string name, object value)
        {
            this.values.Add(name, value);
        }

        /// <summary>
        /// Gets or sets a session value by using the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public override object this[string name]
        {
            get
            {
                return this.values.ContainsKey(name) ? this.values[name] : null;
            }
            set
            {
                this.values[name] = value;
            }
        }

        /// <summary>
        /// Removes all keys and values from the session-state collection.
        /// </summary>
        public override void Clear()
        {
            this.values.Clear();
        }
    }
}
