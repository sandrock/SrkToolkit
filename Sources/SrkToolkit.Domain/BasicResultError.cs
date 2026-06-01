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
    using SrkToolkit.Domain.Internals;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// Result for a domain request.
    /// </summary>
    [DataContract(Namespace = Names.DataContractNamespace)]
    public class BasicResultError : IResultError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicResultError"/> class.
        /// </summary>
        public BasicResultError()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicResultError"/> class.
        /// </summary>
        /// <param name="displayMessage">The display message.</param>
        public BasicResultError(string displayMessage)
        {
            this.DisplayMessage = displayMessage;
        }

        public BasicResultError(string code, string displayMessage)
        {
            this.Code = code;
            this.DisplayMessage = displayMessage;
        }

        public BasicResultError(string code, string displayMessage, string detail)
        {
            this.Code = code;
            this.DisplayMessage = displayMessage;
            this.Detail = detail;
        }

        /// <summary>
        /// Gets or sets the display message.
        /// </summary>
        [DataMember(IsRequired = false, Order = 1)]
        public string DisplayMessage { get; set; }

        /// <summary>
        /// Gets or sets an additional information.
        /// </summary>
        [DataMember(IsRequired = false, Order = 2)]
        public string Detail { get; set; }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        [DataMember(IsRequired = false, Order = 0)]
        public string Code { get; set; }

        public override string ToString()
        {
            return this.Code
                + " " + this.DisplayMessage
                ;
        }
    }
}
