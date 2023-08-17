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
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Resources;
    using SrkToolkit.Domain.Internals;
    using System.Runtime.Serialization;
    
    /// <summary>
    /// A rich error identified by a code.
    /// </summary>
    /// <remarks>
    /// This object will use a ResourceManager (.resx file) to translate error codes to display messages.
    /// In your resource file, index you value using &lt;EnumTypeName&gt;_&lt;EnumKey&gt;
    /// </remarks>
    /// <typeparam name="TEnum"></typeparam>
    [DataContract(Namespace = Names.DataContractNamespace)]
    public class ResultError<TEnum> : IResultError
        where TEnum : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultError{TEnum}"/> class.
        /// </summary>
        public ResultError()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultError{TEnum}"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="displayMessage">The display message.</param>
        public ResultError(TEnum code, string displayMessage)
        {
            this.Code = code;
            this.DisplayMessage = displayMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultError{TEnum}"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
        public ResultError(TEnum code, ResourceManager enumResourceManager)
        {
            this.Code = code;
            this.DisplayMessage = EnumTools.GetDescription(code, enumResourceManager);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultError{TEnum}"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
        /// <param name="culture">The culture.</param>
        public ResultError(TEnum code, ResourceManager enumResourceManager, CultureInfo culture)
        {
            this.Code = code;
            this.DisplayMessage = EnumTools.GetDescription(code, enumResourceManager, culture);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultError{TEnum}"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="displayMessageFormat">The display message format.</param>
        /// <param name="args">The args.</param>
        public ResultError(TEnum code, string displayMessageFormat, params object[] args)
        {
            this.Code = code;
            this.DisplayMessage = string.Format(displayMessageFormat, args);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultError{TEnum}"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="args">The args.</param>
        public ResultError(TEnum code, ResourceManager enumResourceManager, CultureInfo culture, params object[] args)
        {
            this.Code = code;
            this.DisplayMessage = string.Format(EnumTools.GetDescription(code, enumResourceManager, culture), args);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultError{TEnum}"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="enumResourceManager">The ResourceManager in which to find a translation for the code.</param>
        /// <param name="args">The args.</param>
        public ResultError(TEnum code, ResourceManager enumResourceManager, params object[] args)
        {
            this.Code = code;
            this.DisplayMessage = string.Format(EnumTools.GetDescription(code, enumResourceManager), args);
        }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        [DataMember(IsRequired = false, Order = 0)]
        public TEnum Code { get; set; }

        /// <summary>
        /// Gets or sets the display message.
        /// </summary>
        [DataMember(IsRequired = false, Order = 1)]
        public string DisplayMessage { get; set; }

        /// <summary>
        /// Gets or sets an optional error detail.
        /// </summary>
        [DataMember(IsRequired = false, Order = 2)]
        public string Detail { get; set; }

        string IResultError.DisplayMessage
        {
            get { return this.DisplayMessage ?? this.Code.ToString(); }
        }

        string IResultError.Code
        {
            get { return this.Code.ToString(); }
        }

        public override string ToString()
        {
            return this.Code
                + " " + this.DisplayMessage
                ;
        }
    }
}
