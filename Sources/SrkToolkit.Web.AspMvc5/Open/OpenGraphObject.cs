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

namespace SrkToolkit.Web.Open
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Globalization;

    /// <summary>
    /// WARNING: SrkToolkit.Web.Open.* classes are in development. 
    /// The root object to expose OpenGraph information.
    /// </summary>
    /// <remarks>
    /// http://opengraphprotocol.org/
    /// </remarks>
    public class OpenGraphObject
    {
        private List<OpenGraphTag> tags;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGraphObject"/> class.
        /// </summary>
        public OpenGraphObject()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGraphObject"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="uri">The canonical URI of the page.</param>
        public OpenGraphObject(string title, Uri uri)
        {
            this.Tags.Add(new OpenGraphTag(OpenGraphName.KnownNames.OgTitle, title));
            this.Tags.Add(new OpenGraphTag(OpenGraphName.KnownNames.OgUrl, uri.ToString()));
        }
        /*
        public static OpenGraphObject Website
        {
            get { return OpenGraphObject.CreateKnownType("website"); }
        }
        */
        internal IList<OpenGraphTag> Tags
        {
            get { return this.tags ?? (this.tags = new List<OpenGraphTag>()); }
        }

        /// <summary>
        /// Adds the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public void Add(OpenGraphTag tag)
        {
            this.Tags.Add(tag);
        }

        /// <summary>
        /// Sets the audio file.
        /// </summary>
        /// <param name="uri">A URL to an audio file to accompany this object.</param>
        /// <returns></returns>
        public OpenGraphObject SetAudioFile(Uri uri)
        {
            this.SetTag(OpenGraphName.KnownNames.OgAudio, uri.ToString());
            return this;
        }

        /// <summary>
        /// Sets the description.
        /// </summary>
        /// <param name="value">A one to two sentence description of your object.</param>
        /// <returns></returns>
        public OpenGraphObject SetDescription(string value)
        {
            this.SetTag(OpenGraphName.KnownNames.OgDescription, value);
            return this;
        }

        /// <summary>
        /// Sets the determiner.
        /// </summary>
        /// <param name="value">The word that appears before this object's title in a sentence. An enum of (a, an, the, "", auto). If auto is chosen, the consumer of your data should chose between "a" or "an". Default is "" (blank).</param>
        /// <returns></returns>
        public OpenGraphObject SetDeterminer(string value)
        {
            this.SetTag(OpenGraphName.KnownNames.OgDeterminer, value);
            return this;
        }

        /// <summary>
        /// Sets the locale.
        /// </summary>
        /// <param name="culture">The locale these tags are marked up in. Of the format language_TERRITORY. Default is en_US.</param>
        /// <returns></returns>
        public OpenGraphObject SetLocale(CultureInfo culture)
        {
            this.SetTag(OpenGraphName.KnownNames.OgLocale, culture.Name.Replace('-', '_'));
            return this;
        }

        /// <summary>
        /// Sets the locale.
        /// </summary>
        /// <param name="locale">The locale these tags are marked up in. Of the format language_TERRITORY. Default is en_US.</param>
        /// <returns></returns>
        public OpenGraphObject SetLocale(string locale)
        {
            this.SetTag(OpenGraphName.KnownNames.OgLocale, locale.Replace('-', '_'));
            return this;
        }

        /// <summary>
        /// Sets the alternate locales.
        /// </summary>
        /// <param name="cultures">An array of other locales this page is available in.</param>
        /// <returns></returns>
        public OpenGraphObject SetLocaleAlternates(IEnumerable<CultureInfo> cultures)
        {
            foreach (var culture in cultures)
            {
                this.Tags.Add(new OpenGraphTag(OpenGraphName.KnownNames.OgLocaleAlternate, culture.Name.Replace('-', '_')));
            }

            return this;
        }

        /// <summary>
        /// Sets the name of the site.
        /// </summary>
        /// <param name="value">If your object is part of a larger web site, the name which should be displayed for the overall site. e.g., "IMDb".</param>
        /// <returns></returns>
        public OpenGraphObject SetSiteName(string value)
        {
            this.SetTag(OpenGraphName.KnownNames.OgSiteName, value);
            return this;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.ToHtmlString(null, null, null);
        }

        /// <summary>
        /// To the friendly HTML string.
        /// </summary>
        /// <returns></returns>
        public string ToFriendlyHtmlString()
        {
            return this.ToHtmlString("    ", "\n    ", "\n\n");
        }

        /// <summary>
        /// To the HTML string.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns></returns>
        public string ToHtmlString(string prefix, string separator, string suffix)
        {
            if (this.tags == null || this.tags.Count == 0)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            builder.Append(prefix);
            foreach (var tag in this.Tags)
            {
                builder.Append(tag.ToString());
                builder.Append(separator);
            }

            builder.Append(suffix);
            return builder.ToString();
        }

        /// <summary>
        /// To the HTML attribute namespaces.
        /// </summary>
        /// <returns></returns>
        public string ToHtmlAttributeNamespaces()
        {
            if (this.tags == null || this.tags.Count == 0)
            {
                return string.Empty;
            }

            var namespaces = this.tags.GroupBy(t => t.Key.Namespace).Select(g => g.Key);

            var builder = new StringBuilder();
            foreach (var ns in namespaces)
            {
                builder.Append(ns.ToHtmlAttributeString());
            }

            return builder.ToString();
        }

        /// <summary>
        /// Sets the image.
        /// </summary>
        /// <param name="imageUri">The image URI.</param>
        /// <param name="imageSecureUri">The image secure URI.</param>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public OpenGraphObject SetImage(Uri imageUri = null, Uri imageSecureUri = null, string mimeType = null, int? width = null, int? height = null)
        {
            if (imageUri != null)
                this.Add(new OpenGraphTag(new OpenGraphName("image"), imageUri.ToString()));

            if (imageSecureUri != null)
                this.Add(new OpenGraphTag(new OpenGraphName("image:secure_url"), imageSecureUri.ToString()));

            if (mimeType != null)
                this.Add(new OpenGraphTag(new OpenGraphName("image:type"), mimeType));

            if (width != null)
                this.Add(new OpenGraphTag(new OpenGraphName("image:width"), width.Value.ToString(CultureInfo.InvariantCulture)));

            if (height != null)
                this.Add(new OpenGraphTag(new OpenGraphName("image:height"), height.Value.ToString(CultureInfo.InvariantCulture)));

            return this;
        }

        internal OpenGraphObject SetTag(OpenGraphName name, string value)
        {
            var tags = this.Tags.Where(t => t.Key == name).ToArray();

            if (tags.Length == 0)
            {
                this.Tags.Add(new OpenGraphTag(name, value));
            }
            else if (tags.Length == 1)
            {
                tags[0].Value = value;
            }
            else
            {
                this.tags.RemoveAll(t => t.Key == name);
                this.Tags.Add(new OpenGraphTag(name, value));
            }

            return this;
        }

        internal void SetType(string value)
        {
            this.SetTag(OpenGraphName.KnownNames.OgType, value);
        }
    }

    public static class OpenGraphMusicObject
    {
        public static OpenGraphObject Song(string title, Uri uri, TimeSpan? duration = null, string album = null, int? disk = null, int? track = null, string musician = null)
        {
            return new OpenGraphObject(title, uri).IsSong(duration, album, disk, track, musician);
        }

        public static OpenGraphObject IsSong(this OpenGraphObject obj, TimeSpan? duration = null, string album = null, int? disk = null, int? track = null, string musician = null)
        {
            obj.SetType("music.song");

            if (duration != null)
                obj.Tags.Add(new OpenGraphTag(new OpenGraphName("music:duration"), duration.Value.TotalSeconds.ToString(CultureInfo.InvariantCulture)));

            if (disk != null)
                obj.Tags.Add(new OpenGraphTag(new OpenGraphName("music:album:disk"), disk.Value.ToString(CultureInfo.InvariantCulture)));

            if (track != null)
                obj.Tags.Add(new OpenGraphTag(new OpenGraphName("music:album:disk"), track.Value.ToString(CultureInfo.InvariantCulture)));

            if (album != null)
                obj.Tags.Add(new OpenGraphTag(new OpenGraphName("music:album"), album));

            if (musician != null)
                obj.Tags.Add(new OpenGraphTag(new OpenGraphName("music:musician"), musician));

            return obj;
        }
    }

    public static class OpenGraphImageObject
    {
        public static OpenGraphObject Image(string title, Uri uri, Uri imageUri = null, Uri imageSecureUri = null, string mimeType = null, int? width = null, int? height = null)
        {
            return new OpenGraphObject(title, uri).IsImage(imageUri, imageSecureUri, mimeType, width, height);
        }

        public static OpenGraphObject IsImage(this OpenGraphObject obj, Uri imageUri = null, Uri imageSecureUri = null, string mimeType = null, int? width = null, int? height = null)
        {
            if (imageUri != null)
                obj.Add(new OpenGraphTag(new OpenGraphName("image"), imageUri.ToString()));

            if (imageSecureUri != null)
                obj.Add(new OpenGraphTag(new OpenGraphName("image:secure_url"), imageSecureUri.ToString()));

            if (mimeType != null)
                obj.Add(new OpenGraphTag(new OpenGraphName("image:type"), mimeType));

            if (width != null)
                obj.Add(new OpenGraphTag(new OpenGraphName("image:width"), width.Value.ToString(CultureInfo.InvariantCulture)));

            if (height != null)
                obj.Add(new OpenGraphTag(new OpenGraphName("image:height"), height.Value.ToString(CultureInfo.InvariantCulture)));

            return obj;
        }
    }
    /*
    public static class OpenGraphVideoObject
    {
        public static OpenGraphObject Video(string title, Uri uri, Uri imageUri = null, Uri imageSecureUri = null, string mimeType=null, int? width=null, int? height=null)
        {
            return new OpenGraphObject(title, uri).IsVideo(imageUri, imageSecureUri, mimeType, width, height);
        }

        public static OpenGraphObject IsVideo(this OpenGraphObject obj, Uri imageUri = null, Uri imageSecureUri = null, string mimeType = null, int? width = null, int? height = null)
        {
            obj.SetType("music.song");

            if (imageUri != null)
                obj.Add(new OpenGraphTag("image", imageUri.ToString()));

            if (imageSecureUri != null)
                obj.Add(new OpenGraphTag("image:secure_url", imageSecureUri.ToString()));

            if (mimeType != null)
                obj.Add(new OpenGraphTag("image:type", mimeType));

            if (width != null)
                obj.Add(new OpenGraphTag("image:width", width.Value.ToString(CultureInfo.InvariantCulture)));

            if (height != null)
                obj.Add(new OpenGraphTag("image:height", height.Value.ToString(CultureInfo.InvariantCulture)));

            return obj;
        }
    }

    public enum OpenGraphVideoType
    {
        movie,
        episode,
        tv_show,
        other,
    }
    */
}
