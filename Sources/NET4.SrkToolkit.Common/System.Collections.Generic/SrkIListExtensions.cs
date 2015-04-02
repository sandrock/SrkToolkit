
namespace System.Collections.Generic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static  class SrkIListExtensions
    {
        public static T SingleOrCreate<T>(this IList<T> collection, Func<T, bool> predicate, Func<T> create)
            where T : class
        {
            var item = collection.SingleOrDefault(predicate);
            if (item != null)
            {
                return item;
            }
            else
            {
                item = create();
                collection.Add(item);
                return item;
            }
        }

        public static T FirstOrCreate<T>(this IList<T> collection, Func<T, bool> predicate, Func<T> create)
            where T : class
        {
            var item = collection.FirstOrDefault(predicate);
            if (item != null)
            {
                return item;
            }
            else
            {
                item = create();
                collection.Add(item);
                return item;
            }
        }

        public static T LastOrCreate<T>(this IList<T> collection, Func<T, bool> predicate, Func<T> create)
            where T : class
        {
            var item = collection.LastOrDefault(predicate);
            if (item != null)
            {
                return item;
            }
            else
            {
                item = create();
                collection.Add(item);
                return item;
            }
        }
    }
}
