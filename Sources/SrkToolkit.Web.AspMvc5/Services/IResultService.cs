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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// Helps return generic HTTP responses.
    /// </summary>
    public interface IResultService
    {
        /// <summary>
        /// Returns a 403 Forbidden view.
        /// </summary>
        /// <param name="message">a custom message can be specified. leave null for random message.</param>
        ActionResult Forbidden(string message = null);

        /// <summary>
        /// Shows a 404 page.
        /// </summary>
        /// <param name="message">a custom message can be specified. leave null for random message.</param>
        /// <returns></returns>
        ActionResult NotFound(string message = null);

        /// <summary>
        /// Shows a 400 page.
        /// </summary>
        ActionResult BadRequest(string message = null);

        /// <summary>
        /// Returns a standard JSON result for a successful operation.
        /// </summary>
        /// <returns></returns>
        ActionResult JsonSuccess();

        /// <summary>
        /// Returns a standard JSON result containing data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        ActionResult JsonSuccess(object data);

        /// <summary>
        /// Returns a standard JSON result containing an error.
        /// </summary>
        /// <returns></returns>
        ActionResult JsonError();

        /// <summary>
        /// Returns a standard JSON result containing an error.
        /// </summary>
        /// <param name="errorCode">helps identify the the error</param>
        /// <returns></returns>
        ActionResult JsonError(string errorCode);

        /// <summary>
        /// Returns a standard JSON result containing an error.
        /// </summary>
        /// <param name="errorCode">helps identify the the error</param>
        /// <param name="errorMessage">the translated error message to display</param>
        /// <returns></returns>
        ActionResult JsonError(string errorCode, string errorMessage);
        
        /// <summary>
        /// Returns a standard JSON result containing an error.
        /// </summary>
        /// <param name="errorCode">helps identify the the error</param>
        /// <param name="errorMessage">the translated error message to display</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        ActionResult JsonError(string errorCode, string errorMessage, object data);

        /// <summary>
        /// Returns a standard JSON result containing an error.
        /// </summary>
        /// <param name="httpCode">The HTTP code.</param>
        /// <param name="errorCode">helps identify the the error</param>
        /// <param name="errorMessage">the translated error message to display</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        ActionResult JsonError(int httpCode, string errorCode, string errorMessage, object data);
    }
}
