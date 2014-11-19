
namespace SrkToolkit.Web
{
    using System;

    /// <summary>
    /// Ways of display domain error messages in a ASP MVC web app.
    /// </summary>
    public enum MessageDisplayMode
    {
        /// <summary>
        /// Do not display the error. Allows manual handling of error display.
        /// </summary>
        None,

        /// <summary>
        /// Adds atemporary data error message for each error. See <see cref="SrkTempDataDictionaryExtensions.GetAll"/> and <see cref="SrkTempDataDictionaryExtensions.AddError"/>.
        /// </summary>
        TempData,

        /// <summary>
        /// Appends errors to the ModelState.
        /// </summary>
        ModelState,
    }
}
