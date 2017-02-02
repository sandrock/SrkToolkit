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
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using SrkToolkit.Web.HttpErrors;

    /// <summary>
    /// Helps return generic HTTP responses.
    /// </summary>
    public class ResultServiceBase
    {
        private readonly HttpContextBase httpContext;

        public const string RouteDataExceptionKey = "error";
        public const string RouteDataMessageKey = "message";
        public const string RouteDataHttpCodeKey = "http code";

        private int jsonErrorHttpStatusCode = 400;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultServiceBase"/> class.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public ResultServiceBase(HttpContextBase httpContext)
        {
            this.httpContext = httpContext;
        }

        /// <summary>
        /// Gets or sets the json error HTTP status code (default is 400).
        /// </summary>
        public int JsonErrorHttpStatusCode
        {
            get { return this.jsonErrorHttpStatusCode; }
            set { this.jsonErrorHttpStatusCode = value; }
        }

        /// <summary>
        /// Returns a standard JSON result for a successful operation.
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonSuccess()
        {
            return new JsonNetResult
            {
                Data = new
                {
                    Success = true,
                    ErrorCode = default(string),
                    ErrorMessage = default(string),
                    Data = default(string),
                },
            };
        }

        /// <summary>
        /// Returns a standard JSON result containing data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult JsonSuccess(object data)
        {
            return new JsonNetResult
            {
                Data = new
                {
                    Success = true,
                    ErrorCode = default(string),
                    ErrorMessage = default(string),
                    Data = data,
                },
            };
        }

        /// <summary>
        /// Returns a standard JSON result containing an error.
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonError()
        {
            return new JsonNetResult
            {
                HttpStatusCode = this.JsonErrorHttpStatusCode,
                Data = new
                {
                    Success = false,
                    ErrorCode = default(string),
                    ErrorMessage = default(string),
                    Data = default(string),
                },
            };
        }

        /// <summary>
        /// Returns a standard JSON result containing an error.
        /// </summary>
        /// <param name="errorCode">helps identify the the error</param>
        /// <returns></returns>
        public ActionResult JsonError(string errorCode)
        {
            return new JsonNetResult
            {
                HttpStatusCode = this.JsonErrorHttpStatusCode,
                Data = new
                {
                    Success = false,
                    ErrorCode = errorCode,
                    ErrorMessage = default(string),
                    Data = default(string),
                },
            };
        }

        /// <summary>
        /// Returns a standard JSON result containing an error.
        /// </summary>
        /// <param name="errorCode">helps identify the the error</param>
        /// <param name="errorMessage">the translated error message to display</param>
        /// <returns></returns>
        public ActionResult JsonError(string errorCode, string errorMessage)
        {
            return new JsonNetResult
            {
                HttpStatusCode = this.JsonErrorHttpStatusCode,
                Data = new
                {
                    Success = false,
                    ErrorCode = errorCode,
                    ErrorMessage = errorMessage,
                    Data = default(string),
                },
            };
        }

        /// <summary>
        /// Returns a standard JSON result containing an error.
        /// </summary>
        /// <param name="errorCode">helps identify the the error</param>
        /// <param name="errorMessage">the translated error message to display</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public ActionResult JsonError(string errorCode, string errorMessage, object data)
        {
            return new JsonNetResult
            {
                HttpStatusCode = this.JsonErrorHttpStatusCode,
                Data = new
                {
                    Success = false,
                    ErrorCode = errorCode,
                    ErrorMessage = errorMessage,
                    Data = data,
                },
            };
        }

        /// <summary>
        /// Returns a standard JSON result containing an error.
        /// </summary>
        /// <param name="httpCode">The HTTP code.</param>
        /// <param name="errorCode">helps identify the the error</param>
        /// <param name="errorMessage">the translated error message to display</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public ActionResult JsonError(int httpCode, string errorCode, string errorMessage, object data)
        {
            var result = new JsonNetResult
            {
                Data = new
                {
                    Success = false,
                    ErrorCode = errorCode,
                    ErrorMessage = errorMessage,
                    Data = data,
                },
            };
            result.HttpStatusCode = httpCode;
            return result;
        }

        /// <summary>
        /// Gets the HTTP context.
        /// </summary>
        protected HttpContextBase HttpContext
        {
            get { return this.httpContext; }
        }

        internal ActionResult JsonErrorWithException(int httpCode, string errorCode, string errorMessage, Exception exception)
        {
            return new JsonNetResult
            {
                HttpStatusCode = httpCode,
                Data = new
                {
                    Success = false,
                    ErrorCode = errorCode,
                    ErrorMessage = errorMessage,
                    Data = default(string),
                    Exception = DescribeException(exception),
                },
            };
        }

        private object DescribeException(Exception exception)
        {
            if (exception != null)
            {
                return new
                {
                    Type = exception.GetType().FullName,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    Inner = DescribeException(exception.InnerException),
                };
            }
            else
            {
                return null;
            }
        }
    }
}
