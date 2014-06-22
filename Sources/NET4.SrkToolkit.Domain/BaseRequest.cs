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
    /// Allows a domain request to be validated before processing.
    /// </summary>
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
        /// <value>
        /// All validation errors.
        /// </value>
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
        /// <value>
        /// The validation errors grouped by property.
        /// </value>
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
