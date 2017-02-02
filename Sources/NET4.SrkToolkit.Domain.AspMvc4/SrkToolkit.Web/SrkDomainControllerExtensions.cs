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

namespace SrkToolkit.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using SrkToolkit.Domain;

    /// <summary>
    /// Extension methods for <see cref="BaseResult"/> to use with <see cref="Controller"/> actions.
    /// </summary>
    public static class SrkDomainControllerExtensions
    {
        /// <summary>
        /// Validates the result and handle domain errors.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResultCode">The type of the result code.</typeparam>
        /// <param name="controller">The controller.</param>
        /// <param name="result">The result.</param>
        /// <param name="errorDisplayMode">The error display mode.</param>
        /// <param name="modelStatePrefix">The model state prefix.</param>
        /// <returns></returns>
        public static bool ValidateResult<TRequest, TResultCode>(
            this Controller controller, SrkToolkit.Domain.BaseResult<TRequest, TResultCode> result,
            MessageDisplayMode errorDisplayMode = MessageDisplayMode.ModelState,
            string modelStatePrefix = null)
            where TRequest : SrkToolkit.Domain.BaseRequest
            where TResultCode : struct
        {
            ShowResultErrors(controller, result.Errors, errorDisplayMode, modelStatePrefix);

            if (result.Request != null)
            {
                foreach (var item in result.Request.ValidationErrors)
                {
                    foreach (var error in item.Value)
                    {
                        var key = modelStatePrefix != null ? (modelStatePrefix + "." + item.Key) : item.Key;
                        controller.ModelState.AddModelError(key, error);
                    }
                }
            }

            return result.Succeed;
        }

        /// <summary>
        /// Validates the result and handle domain errors.
        /// </summary>
        /// <typeparam name="TResultCode">The type of the result code.</typeparam>
        /// <param name="controller">The controller.</param>
        /// <param name="result">The result.</param>
        /// <param name="errorDisplayMode">The error display mode.</param>
        /// <param name="modelStatePrefix">The model state prefix.</param>
        /// <returns></returns>
        public static bool ValidateResult<TResultCode>(
            this Controller controller,
            SrkToolkit.Domain.BasicResult<TResultCode> result,
            MessageDisplayMode errorDisplayMode = MessageDisplayMode.ModelState,
            string modelStatePrefix = null)
            where TResultCode : struct
        {
            ShowResultErrors(controller, result.Errors, errorDisplayMode, modelStatePrefix);

            return result.Succeed;
        }

        private static void ShowResultErrors<TResultCode>(Controller controller, IList<ResultError<TResultCode>> errors, MessageDisplayMode displayMode, string modelStatePrefix)
            where TResultCode : struct
        {
            if (errors != null && errors.Count > 0)
            {
                for (int i = 0; i < errors.Count; i++)
                {
                    string message = errors[i].DisplayMessage
                                  ?? errors[i].Code.ToString();
                    switch (displayMode)
                    {
                        case MessageDisplayMode.None:
                            break;

                        case MessageDisplayMode.TempData:
                            controller.TempData.AddError(message);
                            break;
                        case MessageDisplayMode.ModelState:
                            controller.ModelState.AddModelError(modelStatePrefix ?? string.Empty, message);
                            break;

                        default:
                            throw new NotSupportedException("Display mode '" + displayMode + "' is not supported.");
                    }
                }
            }
        }
    }
}
