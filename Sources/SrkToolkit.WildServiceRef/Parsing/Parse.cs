using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SrkToolkit.WildServiceRef.Parsing {

    public delegate TReturn ParserDelegate<TSource, out TReturn>(TSource source) where TReturn : class, new();
    public delegate object ParserWeakDelegate<TSource>(TSource source);
    public delegate IList<object> ParserCollWeakDelegate<TSource>(TSource source);

    public static class Parse {

        #region String parsers registration

        internal static readonly Dictionary<string, Dictionary<Guid, ParserWeakDelegate<string>>> stringParsers =
            new Dictionary<string, Dictionary<Guid, ParserWeakDelegate<string>>>();

        internal static readonly Dictionary<string, Dictionary<Guid, ParserCollWeakDelegate<string>>> stringMultiParsers =
            new Dictionary<string, Dictionary<Guid, ParserCollWeakDelegate<string>>>();

        //internal static readonly Dictionary<string, Dictionary<Guid, Func<object, object>>> stringParsers =
        //    new Dictionary<string, Dictionary<Guid, Func<object, object>>>();

        //internal static readonly Dictionary<string, Dictionary<Guid, Func<object, IList<object>>>> stringMultiParsers =
        //    new Dictionary<string, Dictionary<Guid, Func<object, IList<object>>>>();

        #endregion
        
        #region String parsers usage

        //public static Parser<string> String(string source) {
        public static StringParser String(string source) {
            //return new Parser<string>(source);
            return new StringParser(source);
        }
        
        //public static Parser<string> String() {
        public static StringParser String() {
            //return new Parser<string>();
            return new StringParser();
        }

        #endregion

    }

    public class Parser<TSource> {

        internal readonly TSource source;

        public Parser() {

        }

        public Parser(TSource source) {
            this.source = source;
        }

        public SubParser<TSource> As(string format)
        {
            return new SubParser<TSource>(source, format);
        }

    }

    public class StringParser {

        internal readonly string source;

        public StringParser() {

        }

        public StringParser(string source) {
            this.source = source;
        }

        public StringSubParser As(string format) {
            return new StringSubParser(source, format);
        }

    }
    
    public class SubParser<TSource> {

        internal readonly TSource source;

        internal readonly string format;

        public SubParser(string format) {
            this.format = format;
        }

        public SubParser(TSource source, string format) {
            this.source = source;
            this.format = format;
        }
        /*
        public void RegisterStringToOneParser<T>(string format, ParserDelegate<string, T> parser) where T : class {
            if (!Parse.stringParsers.ContainsKey(format))
                Parse.stringParsers.Add(format, new Dictionary<Guid, ParserDelegate<string, object>>());

            //Parse.stringParsers[format]
            //    .Add(typeof(T).GUID, parser);
        }
        
        public void RegisterStringToManyParser<T>(string format, ParserDelegate<string, List<T>> parser) where T : class {
            if (!Parse.stringMultiParsers.ContainsKey(format))
                Parse.stringMultiParsers.Add(format, new Dictionary<Guid, ParserDelegate<string, List<object>>>());

            //Parse.stringMultiParsers[format]
            //    .Add(typeof(T).GUID, parser);
        }

        public T ToOne<T>(TSource source) where T : class {
            //return (T)Parse.stringParsers[format][typeof(T).GUID](source);
        }

        public IList<T> ToMany<T>() where T : class {
            //return (IList<T>)Parse.stringMultiParsers[format][typeof(T).GUID](source);
        }
*/
    }
    
    public class StringSubParser {

        internal readonly string source;

        internal readonly string format;

        public StringSubParser(string format) {
            this.format = format;
        }

        public StringSubParser(string source, string format) {
            this.source = source;
            this.format = format;
        }

        //public void RegisterToOneParser<T>(ParserDelegate<string, T> parser) where T : class, new() {
        //    if (!Parse.stringParsers.ContainsKey(format))
        //        Parse.stringParsers.Add(format, new Dictionary<Guid, ParserDelegate<string, object>>());

        //    Parse.stringParsers[format]
        //        .Add(typeof(T).GUID, parser);
        //}

        //public void RegisterToManyParser<T>(ParserDelegate<string, List<T>> parser) where T : class, new() {
        //    if (!Parse.stringMultiParsers.ContainsKey(format))
        //        Parse.stringMultiParsers.Add(format, new Dictionary<Guid, ParserDelegate<string, List<object>>>());

        //    Parse.stringMultiParsers[format]
        //        .Add(
        //            typeof(T).GUID,
        //            parser);
        //}

        public void RegisterToOneParser<T>(ParserWeakDelegate<string> parser) where T : class, new() {
            if (!Parse.stringParsers.ContainsKey(format))
                Parse.stringParsers.Add(format, new Dictionary<Guid, ParserWeakDelegate<string>>());

            Parse.stringParsers[format]
                .Add(typeof(T).GUID, parser);
        }

        public void RegisterToManyParser<T>(ParserCollWeakDelegate<string> parser) where T : class, new() {
            if (!Parse.stringMultiParsers.ContainsKey(format))
                Parse.stringMultiParsers.Add(format, new Dictionary<Guid, ParserCollWeakDelegate<string>>());

            Parse.stringMultiParsers[format]
                .Add(
                    typeof(T).GUID,
                    parser);
        }

        public T ToOne<T>() where T : class {
            return (T)Parse.stringParsers[format][typeof(T).GUID](source);
        }

        public T ToOne<T>(string source) where T : class {
            return (T)Parse.stringParsers[format][typeof(T).GUID](source);
        }

        public IList<T> ToMany<T>() where T : class {
            return (IList<T>)Parse.stringMultiParsers[format][typeof(T).GUID](source);
        }

        public IList<T> ToMany<T>(string source) where T : class {
            return (IList<T>)Parse.stringMultiParsers[format][typeof(T).GUID](source);
        }

    }

    public static class ParseExtensions {

    }
    /*
    public class Parsers<string> {

    }

    public static class Parse<TSource> where TSource : struct {

        private static readonly Dictionary<Guid, Func<TSource, object>> oneParsers =
            new Dictionary<Guid, Func<TSource, object>>();

        private static readonly Dictionary<Guid, Func<TSource, IList<object>>> multiParsers =
            new Dictionary<Guid, Func<TSource, IList<object>>>();

        public static void RegisterItemParser<T>(Func<TSource, object> parser) {
            oneParsers.Add(typeof(T).GUID, parser);
        }

        public static T One<T>(TSource source) where T : new() {
            var t = typeof(T);
            if (oneParsers.ContainsKey(t.GUID)) {
                var parser = oneParsers[t.GUID];
                return (T)parser(source);
            }
            throw new InvalidOperationException("No parser for type '" + t.Name + "'");
        }
    }
    */
}
