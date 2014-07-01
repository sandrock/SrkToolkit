// -----------------------------------------------------------------------
// <copyright file="OpenGraphNamespace.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.Web.Open
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The namespace of a property name.
    /// </summary>
    public class OpenGraphNamespace
    {
        private static OpenGraphNamespace og = new OpenGraphNamespace("og");
        private static OpenGraphNamespace fb = new OpenGraphNamespace("fb");

        private string name;
        private string uri;

        internal OpenGraphNamespace(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("The value cannot be empty", "name");

            this.name = name;
            switch (name)
            {
                case "og":
                    this.uri = "http://ogp.me/ns#";
                    break;

                case "fb":
                    this.uri = "https://www.facebook.com/2008/fbml";
                    break;

                default:
                    throw new ArgumentException("name is not supported", "name");
            }
        }

        public OpenGraphNamespace(string name, string uri)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("The value cannot be empty", "name");

            if (string.IsNullOrEmpty(uri))
                throw new ArgumentException("The value cannot be empty", "uri");

            this.name = name;
            this.uri = uri;
        }

        /// <summary>
        /// Gets the OpenGraph namespace.
        /// </summary>
        public static OpenGraphNamespace OG
        {
            get { return og; }
        }

        /// <summary>
        /// Gets the FaceBook namespace.
        /// </summary>
        public static OpenGraphNamespace FB
        {
            get { return fb; }
        }

        /// <summary>
        /// Gets the name of the namespace.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the URI.
        /// </summary>
        public string Uri
        {
            get { return this.uri; }
        }

        ////public static implicit operator OpenGraphNamespace(string name)
        ////{
        ////    return new OpenGraphNamespace(name);
        ////}

        public static bool operator ==(OpenGraphNamespace left, OpenGraphNamespace right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;

            if (ReferenceEquals(left, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(OpenGraphNamespace left, OpenGraphNamespace right)
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
            return this.name;
        }

        /// <summary>
        /// To the HTML attribute string.
        /// </summary>
        /// <returns></returns>
        public string ToHtmlAttributeString()
        {
            return " xmlns:" + this.name.ProperHtmlAttributeEscape() + "=\"" + this.uri.ProperHtmlAttributeEscape() + "\" ";
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

            if (obj is OpenGraphNamespace)
            {
                var o = (OpenGraphNamespace)obj;
                return o.Name == this.name && o.uri == this.uri;
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
            return (this.name ?? string.Empty).GetHashCode() & (this.uri ?? string.Empty).GetHashCode() >> 16;
        }
    }
}
