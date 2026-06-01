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

    /// <summary>
    /// The name for a property.
    /// </summary>
    public class OpenGraphName
    {
        private OpenGraphNamespace @namespace;
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGraphName"/> class in the "og" namespace.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentException">The value cannot be empty;name</exception>
        public OpenGraphName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("The value cannot be empty", "name");

            this.@namespace = OpenGraphNamespace.OG;
            if (name.StartsWith("og:"))
            {
                this.name = name.Substring(3);
            }
            else
            {
                this.name = name;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGraphName"/> class in the specified namespace.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentException">The value cannot be empty;name</exception>
        /// <exception cref="System.ArgumentNullException">@namespace</exception>
        public OpenGraphName(OpenGraphNamespace @namespace, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("The value cannot be empty", "name");
            if (@namespace == null)
                throw new ArgumentNullException("@namespace");

            this.@namespace = @namespace;
            this.name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGraphName"/> class in the specified namespace.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="namespaceName">Name of the namespace.</param>
        /// <param name="namespaceUri">The namespace URI.</param>
        /// <exception cref="System.ArgumentException">The value cannot be empty;name</exception>
        public OpenGraphName(string name, string namespaceName, string namespaceUri)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("The value cannot be empty", "name");

            this.@namespace = new OpenGraphNamespace(namespaceName, namespaceUri);
            this.name = name;
        }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        /// <value>
        /// The namespace.
        /// </value>
        public OpenGraphNamespace Namespace
        {
            get { return this.@namespace; }
        }

        /// <summary>
        /// Gets the name of the namespace.
        /// </summary>
        /// <value>
        /// The name of the namespace.
        /// </value>
        public string NamespaceName
        {
            get { return this.@namespace.Name; }
        }

        /// <summary>
        /// Gets the name (without the namespace).
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get { return this.name; }
        }

        ////public static implicit operator OpenGraphName(string name)
        ////{
        ////    if (name.StartsWith("og:"))
        ////    {
        ////        return new OpenGraphName("og", name.Substring(3));
        ////    }
        ////    else
        ////    {
        ////        return new OpenGraphName("og", name);
        ////    }
        ////}

        public static bool operator ==(OpenGraphName left, OpenGraphName right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;

            if (ReferenceEquals(left, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(OpenGraphName left, OpenGraphName right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return false;

            if (ReferenceEquals(left, null))
                return true;

            return !left.Equals(right);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.@namespace.Name + ":" + this.name;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj is OpenGraphName)
            {
                var o = (OpenGraphName)obj;
                return o.Name == this.name && o.NamespaceName == this.NamespaceName;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return (this.name ?? string.Empty).GetHashCode() & (this.@namespace ?? new OpenGraphNamespace(string.Empty)).GetHashCode() >> 16;
        }

        /// <summary>
        /// Known property names/
        /// </summary>
        public static class KnownNames
        {
            /// <summary>
            /// The opengraph name for the title of your object as it should appear within the graph.
            /// </summary>
            public static OpenGraphName OgTitle = new OpenGraphName("og:title");

            /// <summary>
            /// The opengraph name for the type of your object.
            /// </summary>
            public static OpenGraphName OgType = new OpenGraphName("og:type");

            /// <summary>
            /// The opengraph name for the canonical URL of your object that will be used as a permanent ID in the graph.
            /// </summary>
            public static OpenGraphName OgUrl = new OpenGraphName("og:url");

            /// <summary>
            /// The opengraph name for the URL to an audio file to accompany this object.
            /// </summary>
            public static OpenGraphName OgAudio = new OpenGraphName("og:audio");

            /// <summary>
            /// The opengraph name for the word that appears before this object's title in a sentence.
            /// </summary>
            public static OpenGraphName OgDeterminer = new OpenGraphName("og:determiner");

            /// <summary>
            /// The opengraph name for the description of your object in one or two sentences.
            /// </summary>
            public static OpenGraphName OgDescription = new OpenGraphName("og:description");

            /// <summary>
            /// The opengraph name for the local of the opengraph tags.
            /// </summary>
            public static OpenGraphName OgLocale = new OpenGraphName("og:locale");

            /// <summary>
            /// The opengraph name for the another locale this page is available in.
            /// </summary>
            public static OpenGraphName OgLocaleAlternate = new OpenGraphName("og:locale:alternate");

            /// <summary>
            /// The opengraph name for the name of the website displaying your object.
            /// </summary>
            public static OpenGraphName OgSiteName = new OpenGraphName("og:site_name");
        }
    }
}
