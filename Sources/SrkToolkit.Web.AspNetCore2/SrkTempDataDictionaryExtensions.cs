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
#if ASPMVCCORE
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#endif
    
#if ASPMVC
    using System.Web;
    using System.Web.Mvc;
#endif

    using SrkToolkit.Web.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Extensions methods for <see cref="TempDataDictionary"/>.
    /// </summary>
    public static class SrkTempDataDictionaryExtensions
    {
        /// <summary>
        /// Adds an error to the temporary data.
        /// </summary>
        /// <param name="tempData">The temp data.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="isMarkup">indicates the message contains HTML markup</param>
        public static void AddError(
#if ASPMVC
            this TempDataDictionary tempData,
#elif ASPMVCCORE
            this ITempDataDictionary tempData,
#endif
            string errorMessage, bool isMarkup = false)
        {
            var message = new TempMessage(TempMessageKind.Error, errorMessage, isMarkup);

            AddMessage(tempData, message);
        }

        /// <summary>
        /// Adds a warning to the temporary data.
        /// </summary>
        /// <param name="tempData">The temp data.</param>
        /// <param name="warningMessage">The warning message.</param>
        /// <param name="isMarkup">indicates the message contains HTML markup</param>
        public static void AddWarning(
#if ASPMVC
            this TempDataDictionary tempData,
#elif ASPMVCCORE
            this ITempDataDictionary tempData,
#endif
            string warningMessage, bool isMarkup = false)
        {
            var message = new TempMessage(TempMessageKind.Warning, warningMessage, isMarkup);

            AddMessage(tempData, message);
        }

        /// <summary>
        /// Adds an info to the temporary data.
        /// </summary>
        /// <param name="tempData">The temp data.</param>
        /// <param name="infoMessage">The info message.</param>
        /// <param name="isMarkup">indicates the message contains HTML markup</param>
        public static void AddInfo(
#if ASPMVC
            this TempDataDictionary tempData,
#elif ASPMVCCORE
            this ITempDataDictionary tempData,
#endif
            string infoMessage, bool isMarkup = false)
        {
            var message = new TempMessage(TempMessageKind.Information, infoMessage, isMarkup);

            AddMessage(tempData, message);
        }

        /// <summary>
        /// Adds a confirmation to the temporary data.
        /// </summary>
        /// <param name="tempData">The temp data.</param>
        /// <param name="message">The info message.</param>
        /// <param name="isMarkup">indicates the message contains HTML markup</param>
        public static void AddConfirmation(
#if ASPMVC
            this TempDataDictionary tempData,
#elif ASPMVCCORE
            this ITempDataDictionary tempData,
#endif
            string message, bool isMarkup = false)
        {
            var msg = new TempMessage(TempMessageKind.Confirmation, message, isMarkup);

            AddMessage(tempData, msg);
        }

        /// <summary>
        /// Gets all the temporary messages.
        /// </summary>
        /// <param name="tempData">The temp data.</param>
        /// <returns></returns>
        public static IList<TempMessage> GetAll(
#if ASPMVC
            this TempDataDictionary tempData
#elif ASPMVCCORE
            this ITempDataDictionary tempData
#endif
            )
        {
            var list = new List<TempMessage>();

            if (tempData.ContainsKey(TempMessage.TempDataKey))
            {
                var errors = tempData[TempMessage.TempDataKey] as IList<TempMessage>;
                if (errors != null)
                    list.AddRange(errors);
            }
            
            return list;
        }

        private static void AddMessage(
#if ASPMVC
            this TempDataDictionary tempData,
#elif ASPMVCCORE
            this ITempDataDictionary tempData,
#endif
            TempMessage message)
        {
            IList<TempMessage> list = null;

            if (tempData.ContainsKey(TempMessage.TempDataKey))
            {
                list = tempData[TempMessage.TempDataKey] as IList<TempMessage>;
                if (list != null)
                {
                    list.Add(message);
                }
            }

            if (list == null)
            {
                list = new List<TempMessage>();
                list.Add(message);
                tempData.Add(TempMessage.TempDataKey, list);
            }
        }
    }
}
