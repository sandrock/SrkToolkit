
namespace SrkToolkit.Services
{
    using System;

    public enum AlreadyRegisteredBehavior
    {
        /// <summary>
        /// Will throw an <see cref="InvalidOperationException"/> if a registration exists for a type.
        /// </summary>
        Throw,

        /// <summary>
        /// Will overwrite registration if a registration exists for a type.
        /// </summary>
        Overwrite,
    }
}
