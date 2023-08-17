﻿// 
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
    /// Allows a domain request to be validated before processing.
    /// </summary>
    [DataContract(Namespace = Names.DataContractNamespace)]
    public class BaseRequest
    {
        private Dictionary<string, List<string>> validationErrors;
        private bool? validated;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRequest"/> class.
        /// </summary>
        public BaseRequest()
        {
        }

        /// <summary>
        /// Gets all validation errors.
        /// </summary>
        [IgnoreDataMember]
        public IEnumerable<string> AllValidationErrors
        {
            get
            {
                if (this.validationErrors == null)
                    return Enumerable.Empty<string>();
                return validationErrors.SelectMany(x => x.Value);
            }
        }

        /// <summary>
        /// Gets the validation errors grouped by property.
        /// </summary>
        [IgnoreDataMember]
        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> ValidationErrors
        {
            get
            {
                if (this.validationErrors == null)
                    yield break;
                foreach (var pair in validationErrors)
                {
                    yield return new KeyValuePair<string, IEnumerable<string>>(pair.Key, pair.Value);
                }
            }
        }

        /// <summary>
        /// Gets the validation error list.
        /// </summary>
        [DataMember]
        protected Dictionary<string, List<string>> ValidationErrorList
        {
            get { return this.validationErrors ?? (this.validationErrors = new Dictionary<string, List<string>>()); }
            set { }
        }

        /// <summary>
        /// Indicates whether the request is valid.
        /// Will execute validation if it has not occured.
        /// </summary>
        [DataMember]
        public bool IsValid
        {
            get
            {
                if (!this.validated.HasValue)
                    this.Validate();
                return this.validated.Value;
            }
            set { }
        }

        /// <summary>
        /// Executes the validation for this model.
        /// </summary>
        /// <returns>Indicates whether the model is valid</returns>
        public bool Validate()
        {
            if (this.validated != null)
            {
                this.ValidationErrorList.Clear();
            }

            this.validated = this.ValidateCore() && this.ValidationErrorList.Count == 0;
            return this.validated.Value;
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if invalid.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The request has validation errors. Refer to the ValidationErrors property.</exception>
        public void ThrowIfInvalid()
        {
            if (!this.IsValid)
                throw new InvalidOperationException("The request has validation errors. Refer to the ValidationErrors property.");
        }

        /// <summary>
        /// Adds a validation error.
        /// </summary>
        /// <param name="key">specify the key name or null</param>
        /// <param name="message"></param>
        public void AddValidationError(string key, string message)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (message == null)
                throw new ArgumentNullException("message");

            List<string> col = null;

            if (this.ValidationErrorList.ContainsKey(key))
                col = this.ValidationErrorList[key];
            else
                this.ValidationErrorList.Add(key, col = new List<string>());

            col.Add(message);
        }

        /// <summary>
        /// Adds a validation error.
        /// </summary>
        /// <param name="message"></param>
        public void AddValidationError(string message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            this.AddValidationError(string.Empty, message);
        }

        /// <summary>
        /// Method to customize the validation process.
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateCore()
        {
            this.ValidateFields();
            return true;
        }

        /// <summary>
        /// Method to override or customize the validation of fields.
        /// </summary>
        /// <returns></returns>
        protected virtual void ValidateFields()
        {
        }
    }
}
