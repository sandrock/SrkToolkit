
namespace SrkToolkit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Resources;
    using System.Globalization;

    public static class EnumTools
    {
        public static string GetDescription<TEnum>(TEnum value, ResourceManager resourceManager)
            where TEnum : struct
        {
            return GetDescription(value, resourceManager, null);
        }

        public static string GetDescription<TEnum>(TEnum value, ResourceManager resourceManager, CultureInfo culture)
            where TEnum : struct
        {
            if (resourceManager == null)
                throw new ArgumentNullException("resourceManager");

            string key = value.GetType().Name + "_" + value.ToString();
            string result = resourceManager.GetString(key, culture);
            return result ?? value.ToString();
        }
    }
}
