
namespace SrkToolkit.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Resources;
    using System.Text;
    using SrkToolkit.Domain.Internals;
    
    /// <summary>
    /// A rich error identified by a code.
    /// </summary>
    /// <remarks>
    /// This object will use a ResourceManager (.resx file) to translate error codes to display messages.
    /// In your resource file, index you value using &lt;EnumTypeName&gt;_&lt;EnumKey&gt;
    /// </remarks>
    /// <typeparam name="TEnum"></typeparam>
    public class ResultError<TEnum> : IResultError
        where TEnum : struct
    {
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
        public TEnum Code { get; set; }

        /// <summary>
        /// Gets or sets the display message.
        /// </summary>
        public string DisplayMessage { get; set; }

        string IResultError.DisplayMessage
        {
            get { return this.DisplayMessage ?? this.Code.ToString(); }
        }

        string IResultError.Code
        {
            get { return this.Code.ToString(); }
        }
    }
}
