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
    /// Incudes a request store, a code-based error list and a success boolean.
    /// </summary>
    [DataContract(Namespace = Names.DataContractNamespace)]
    public class BaseResult<TRequest, TResultCode> : IBaseResult
        where TRequest : class
        where TResultCode : struct
    {
        private TRequest request;
        private IList<ResultError<TResultCode>> errors;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResult{TRequest, TResultCode}"/> class.
        /// </summary>
        public BaseResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResult{TRequest, TResultCode}"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public BaseResult(TRequest request)
        {
            this.request = request;
        }

        /// <summary>
        /// Gets the request.
        /// </summary>
        [DataMember(IsRequired = false, Order = 2)]
        public TRequest Request
        {
            get { return this.request; }
            set { this.request = value; }
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        [DataMember(IsRequired = false, Order = 1)]
        public IList<ResultError<TResultCode>> Errors
        {
            get { return this.errors ?? (this.errors = new List<ResultError<TResultCode>>()); }
            set { this.errors = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the operation succeeded.
        /// </summary>
        [DataMember(IsRequired = false, Order = 0)]
        public bool Succeed { get; set; }

        /// <summary>
        /// Gets a copy of the errors' collection.
        /// </summary>
        IList<IResultError> IBaseResult.Errors
        {
            get
            {
                if (this.errors != null)
                    return new List<IResultError>(this.errors);
                else
                    return new List<IResultError>(0);
            }
        }
    }
}
