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

namespace SrkToolkit.Web.Models
{
    using System;
    using System.Resources;

    // Names and definitions for HTTP status codes are stored in Properties/HttpErrorMessages.resx.
    // Keys: N{code} for the name, D{code} for the description (e.g. N404, D404).
    // To add a locale, add Properties/HttpErrorMessages.{culture}.resx alongside the base file.

    /// <summary>
    /// ViewModel representing an HTTP error with details and description.
    /// </summary>
    public class HttpErrorModel
    {
        private static readonly ResourceManager Resources = new ResourceManager(
            "SrkToolkit.Web.Properties.HttpErrorMessages",
            typeof(HttpErrorModel).Assembly);

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpErrorModel"/> class.
        /// </summary>
        public HttpErrorModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpErrorModel"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        public HttpErrorModel(string title, string message)
        {
            this.Title = title;
            this.Message = message;
        }

        /// <summary>Gets or sets the main page title.</summary>
        public string Title { get; set; }

        /// <summary>Gets or sets the main message.</summary>
        public string Message { get; set; }

        /// <summary>Gets or sets the HTTP code.</summary>
        public int Code { get; set; }

        /// <summary>Gets or sets the URL path.</summary>
        public string UrlPath { get; set; }

        /// <summary>Gets or sets the top link title.</summary>
        public string TopLinkTitle { get; set; }

        /// <summary>Gets or sets the top link href.</summary>
        public string TopLinkHref { get; set; }

        /// <summary>Gets or sets the error action.</summary>
        public string ErrorAction { get; set; }

        /// <summary>Gets or sets the exception.</summary>
        public Exception Exception { get; set; }

        /// <summary>Gets or sets the human-readable code name (e.g. "Not Found").</summary>
        public string CodeName { get; set; }

        /// <summary>Gets or sets the code definition sentence.</summary>
        public string CodeDefinition { get; set; }

        /// <summary>
        /// Creates a model for the given HTTP status code, populating
        /// <see cref="Code"/>, <see cref="CodeName"/> and <see cref="CodeDefinition"/>
        /// from the embedded resource file.
        /// </summary>
        public static HttpErrorModel Create(int code, string title, string message)
        {
            return new HttpErrorModel(title, message)
            {
                Code           = code,
                CodeName       = Resources.GetString("N" + code),
                CodeDefinition = Resources.GetString("D" + code),
            };
        }
    }
}
