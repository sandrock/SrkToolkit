// 
// Copyright 2014 SandRock
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

namespace SrkToolkit.Globalization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Globalization;

    /// <summary>
    /// Utility methods for the <see cref="CultureInfo"/> class.
    /// </summary>
    public static class CultureInfoHelper
    {
        private static readonly Dictionary<CultureInfo, IList<RegionInfo>> countriesCache = new Dictionary<CultureInfo, IList<RegionInfo>>();
        private static readonly object countriesCacheLock = new object();

        /// <summary>
        /// Based on Windows CultureInfos, returns a list of all countries.
        /// </summary>
        /// <returns></returns>
        public static IList<RegionInfo> GetCountries()
        {
            if (countriesCache.ContainsKey(CultureInfo.CurrentCulture))
                return countriesCache[CultureInfo.CurrentCulture];

            var list = new List<Tuple<RegionInfo, CultureInfo>>();
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures & ~CultureTypes.SpecificCultures);
            foreach (var culture in cultures)
            {
                if (!culture.IsNeutralCulture && culture.LCID != 0x7F)
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
                            var bestCultureNameMatch = region.Name + "-" + region.Name;
                            if (!inList.Item2.Name.Equals(bestCultureNameMatch, StringComparison.OrdinalIgnoreCase) && culture.Name.Equals(bestCultureNameMatch, StringComparison.OrdinalIgnoreCase))
                            {
                                list[index] = new Tuple<RegionInfo, CultureInfo>(region, culture);
                            }
                            else if (!inList.Item2.Name.StartsWith("en") && culture.Name.StartsWith("en"))
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

            var list1 = list.Select(t => t.Item1).OrderBy(i => i.NativeName).ToList();
            lock (countriesCacheLock)
            {
                if (!countriesCache.ContainsKey(CultureInfo.CurrentCulture))
                {
                    countriesCache.Add(CultureInfo.CurrentCulture, list1);
                }
            }

            return list1;
        }

        /// <summary>
        /// Based on all CultureInfos (Windows & custom), says whether a culture exists or not.
        /// </summary>
        /// <returns></returns>
        public static bool DoesCultureExist(string cultureName, bool considerInvariantCulture = true)
        {
            if (cultureName == null || !considerInvariantCulture && string.IsNullOrEmpty(cultureName))
            {
                return false;
            }

            return CultureInfo.GetCultures(CultureTypes.AllCultures).Any(culture => cultureName.Equals(culture.Name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
