
namespace SrkToolkit.Globalization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Globalization;

    /// <summary>
    /// 
    /// </summary>
    public static class CultureInfoHelper
    {
        /// <summary>
        /// Based on Windows CultureInfos, returns a list of all countries.
        /// </summary>
        /// <param name="selected">The selected.</param>
        /// <returns></returns>
        public static IList<RegionInfo> GetCountries()
        {
            var list = new List<Tuple<RegionInfo, CultureInfo>>();
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
            foreach (var culture in cultures)
            {
                if (!culture.IsNeutralCulture)
                {
                    try
                    {
                        var region = new RegionInfo(culture.LCID);
                        var inList = list.SingleOrDefault(t => t.Item1.GeoId == region.GeoId);
                        int index = inList != null ? list.IndexOf(inList) : -1;
                        if (inList == null)
                        {
                            list.Add(new Tuple<RegionInfo, CultureInfo>(region, culture));
                        }
                        else
                        {
                            if (!inList.Item2.Name.StartsWith("en") && culture.Name.StartsWith("en"))
                            {
                                list[index] = new Tuple<RegionInfo, CultureInfo>(region, culture);
                            }
                        }
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }

            return list.Select(t => t.Item1).OrderBy(i => i.NativeName).ToList();
        }
    }
}
