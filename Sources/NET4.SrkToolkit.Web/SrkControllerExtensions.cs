
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

        /// <summary>
        /// Gets the <see cref="NavigationLine" /> associated to the request.
        /// </summary>
        /// <param name="ctrl">The control.</param>
        /// <returns></returns>
        public static SrkToolkit.Web.NavigationLine NavigationLine(this Controller ctrl)
        {
            if (ctrl.HttpContext == null)
                throw new ArgumentNullException("HttpContext is not set", "ctrl");

            var line = ctrl.HttpContext.Items[NavigationLineKey] as NavigationLine;
            if (line == null)
            {
                line = new NavigationLine();
                ctrl.HttpContext.Items[NavigationLineKey] = line;
            }

            return line;
        }

        /// <summary>
        /// Gets an item from the HTTP cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctrl">The controller.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="buildData">The build data.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets an item from the HTTP cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctrl">The controller.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="buildData">The build data.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Clears an item from the HTTP cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctrl">The controller.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Clears an item from the HTTP cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctrl">The control.</param>
        /// <returns></returns>
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
