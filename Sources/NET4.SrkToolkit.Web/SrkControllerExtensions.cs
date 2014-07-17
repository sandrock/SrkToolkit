
namespace SrkToolkit.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Caching;
    using System.Web.Mvc;

    /// <summary>
    /// Extension methods for the <see cref="Controller"/> class.
    /// </summary>
    public static class SrkControllerExtensions
    {
        internal const string NavigationLineKey = "SrkNavigationLine";

        public static NavigationLine NavigationLine(this Controller ctrl)
        {
            var line = ctrl.HttpContext.Items[NavigationLineKey] as NavigationLine;
            if (line == null)
            {
                line = new NavigationLine();
                ctrl.HttpContext.Items[NavigationLineKey] = line;
            }

            return line;
        }

        [Obsolete("Under devleopment")]
        public static T GetFromCache<T>(this Controller ctrl, TimeSpan duration, CacheItemPriority priority, Func<T> buildData, string id)
            where T : class
        {
            var cache = ctrl.HttpContext.Cache;
            var key = GetCacheKey<T>(id);
            var value = cache[key] as T;
            if (value == null)
            {
                value = buildData();
                cache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, duration, priority, null);
            }

            return value;
        }

        [Obsolete("Under devleopment")]
        public static T GetFromCache<T>(this Controller ctrl, TimeSpan duration, CacheItemPriority priority, Func<T> buildData)
            where T : class
        {
            var cache = ctrl.HttpContext.Cache;
            var key = GetCacheKey<T>(null);
            var value = cache[key] as T;
            if (value == null)
            {
                value = buildData();
                cache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, duration, priority, null);
            }

            return value;
        }

        [Obsolete("Under devleopment")]
        public static T ClearFromCache<T>(this Controller ctrl, string id)
            where T : class
        {
            var cache = ctrl.HttpContext.Cache;
            var key = GetCacheKey<T>(id);
            var value = cache[key] as T;
            if (value != null)
            {
                cache.Remove(key);
                return value;
            }

            return null;
        }

        [Obsolete("Under devleopment")]
        public static T ClearFromCache<T>(this Controller ctrl)
            where T : class
        {
            var cache = ctrl.HttpContext.Cache;
            var key = GetCacheKey<T>(null);
            var value = cache[key] as T;
            if (value != null)
            {
                cache.Remove(key);
                return value;
            }

            return null;
        }

        private static string GetCacheKey<T>(string id) where T : class
        {
            var type = typeof(T);
            var key = "SrkControllerExtensionsCache-" + type.GUID + "-" + type.FullName + "-" + (id ?? "");
            return key;
        }
    }
}
