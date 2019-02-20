
namespace SrkToolkit.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal static class NetstandardFixes
    {
        internal static bool IsAssignableFrom(this Type type, Type type2)
        {
            throw new NotSupportedException();
        }
    }
}
