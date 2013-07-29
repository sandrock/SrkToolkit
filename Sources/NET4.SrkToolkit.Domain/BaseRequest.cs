// -----------------------------------------------------------------------
// <copyright file="BaseRequest.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseRequest
    {
        private Dictionary<string, List<string>> validationErrors;
        private bool? validated;

        public BaseRequest()
        {
        }

        public IEnumerable<string> AllValidationErrors
        {
            get { return validationErrors.SelectMany(x => x.Value); }
        }

        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> ValidationErrors
        {
            get
            {
                foreach (var pair in validationErrors)
                {
                    yield return new KeyValuePair<string, IEnumerable<string>>(pair.Key, pair.Value);
                }
            }
        }

        protected Dictionary<string, List<string>> ValidationErrorList
        {
            get { return this.validationErrors ?? (this.validationErrors = new Dictionary<string, List<string>>()); }
        }

        /// <summary>
        /// Indicates whether the model is valid.
        /// Will execute validation if it has not occured.
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (!validated.HasValue)
                    Validate();
                return validated.Value;
            }
        }

        /// <summary>
        /// Executes the validation for this model.
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            this.ValidationErrorList.Clear();
            this.validated = this.ValidateCore() && this.validationErrors.Count == 0;
            return this.validated.Value;
        }

        /// <summary>
        /// Adds a validation error.
        /// </summary>
        /// <param name="key">specify the key name or null</param>
        /// <param name="message"></param>
        protected void AddValidationError(string key, string message)
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
        protected void AddValidationError(string message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            AddValidationError(string.Empty, message);
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
