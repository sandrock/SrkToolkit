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

namespace SrkToolkit.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// A message to display at the next loaded web page.
    /// </summary>
    public class TempMessage
    {
        /// <summary>
        /// The key to use to store temp messages.
        /// </summary>
        public const string TempDataKey = "TempMessages";

        /// <summary>
        /// Initializes a new instance of the <see cref="TempMessage"/> class.
        /// </summary>
        public TempMessage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TempMessage"/> class.
        /// </summary>
        /// <param name="kind">The kind.</param>
        /// <param name="message">The message.</param>
        /// <param name="isMarkup">if set to <c>true</c> the <see cref="Message"/> contains HTML markup.</param>
        public TempMessage(TempMessageKind kind, string message, bool isMarkup)
        {
            this.Kind = kind;
            this.Message = message;
            this.IsMarkup = isMarkup;
        }

        /// <summary>
        /// Gets or sets the message (text or markup).
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the kind (Error, Warning, Info).
        /// </summary>
        public TempMessageKind Kind { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Message"/> contains HTML markup.
        /// </summary>
        public bool IsMarkup { get; set; }
    }

    /// <summary>
    /// The type of message.
    /// </summary>
    public enum TempMessageKind
    {
        /// <summary>
        /// A generic information that is not important.
        /// </summary>
        Information,

        /// <summary>
        /// An error that prevented something to happen.
        /// </summary>
        Error,

        /// <summary>
        /// A generic confirmation that an action successfully completed.
        /// </summary>
        Confirmation,

        /// <summary>
        /// A warning that may be important to the user.
        /// </summary>
        Warning,
    }
}