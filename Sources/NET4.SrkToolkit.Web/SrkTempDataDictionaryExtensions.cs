
namespace SrkToolkit.Web
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using SrkToolkit.Web.Models;

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
        public static void AddError(this TempDataDictionary tempData, string errorMessage, bool isMarkup = false)
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
        public static void AddWarning(this TempDataDictionary tempData, string warningMessage, bool isMarkup = false)
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
        public static void AddInfo(this TempDataDictionary tempData, string infoMessage, bool isMarkup = false)
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
        public static void AddConfirmation(this TempDataDictionary tempData, string message, bool isMarkup = false)
        {
            var msg = new TempMessage(TempMessageKind.Confirmation, message, isMarkup);

            AddMessage(tempData, msg);
        }

        /// <summary>
        /// Gets all the temporary messages.
        /// </summary>
        /// <param name="tempData">The temp data.</param>
        /// <returns></returns>
        public static IList<TempMessage> GetAll(this TempDataDictionary tempData)
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

        private static void AddMessage(TempDataDictionary tempData, TempMessage message)
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
