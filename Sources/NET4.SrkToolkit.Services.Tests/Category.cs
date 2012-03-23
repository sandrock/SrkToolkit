
namespace System
{
    public static class Category
    {
        /// <summary>
        /// Represents tests runable at the unit level (environment independant).
        /// </summary>
        public const string Unit = "UnitTest";

        /// <summary>
        /// Represents tests runable at the integration level (environment dependant).
        /// </summary>
        public const string Integration = "IntegrationTest";

        /// <summary>
        /// Represents tests runable at the system level (environment dependant).
        /// </summary>
        public const string System = "SystemTest";

    }
}
