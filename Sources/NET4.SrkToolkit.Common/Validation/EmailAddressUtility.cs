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

namespace SrkToolkit.Common.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents an email address.
    /// </summary>
    public class EmailAddress : IEquatable<EmailAddress>
    {
        private readonly string original;
        private readonly string address;
        private readonly string localPart;
        private readonly string domainPart;
        private readonly string accountPart;
        private readonly string tagPart;

        /// <summary>
        /// Creates a new <see cref="EmailAddress"/> using the specified email address.
        /// Parts of an email address are: AccountPart@DomainPart.
        /// Sub-parts of an email address are: LocalPart+TagPart@DomainPart
        /// </summary>
        /// <param name="address"></param>
        /// <exception cref="ArgumentException">argument is invalid</exception>
        public EmailAddress(string address)
            : this(address, false)
        {
        }

        /// <summary>
        /// Creates a new <see cref="EmailAddress"/> using the specified email address parts.
        /// </summary>
        /// <param name="accountPart">The account part (required).</param>
        /// <param name="tagPart">The tag part.</param>
        /// <param name="domainPart">The domain part (required).</param>
        /// <exception cref="System.ArgumentException">
        /// The value cannot be empty;localPart
        /// or
        /// The value cannot be empty;domainPart
        /// or
        /// Invalid email address;address
        /// </exception>
        public EmailAddress(string accountPart, string tagPart, string domainPart)
        {
            if (string.IsNullOrEmpty(accountPart))
                throw new ArgumentException("The value cannot be empty", "localPart");
            if (string.IsNullOrEmpty(domainPart))
                throw new ArgumentException("The value cannot be empty", "domainPart");

            this.domainPart = domainPart;
            this.accountPart = accountPart;

            string address;
            if (string.IsNullOrWhiteSpace(tagPart))
            {
                this.tagPart = null;
                this.localPart = accountPart;
                address = accountPart + "@" + domainPart;
            }
            else
            {
                this.tagPart = tagPart.Trim();
                this.localPart = accountPart + "+" + this.tagPart;
                address = accountPart + "+" + tagPart + "@" + domainPart;
            }

            address = Validate.EmailAddress(address);
            if (address == null)
                throw new ArgumentException("Invalid email address", "address");

            this.address = address;
        }

        private EmailAddress(string address, bool skipValidation)
        {
            this.original = address;

            if (!skipValidation)
            {
                address = Validate.EmailAddress(address);

                if (address == null)
                    throw new ArgumentException("Invalid email address", "address");
            }

            this.address = address;

            this.localPart = GetLocalPart(address);
            this.domainPart = GetDomainPart(address);
            this.accountPart = GetAccountPart(this.localPart);
            this.tagPart = GetTagPart(this.localPart);
            
        }

        /// <summary>
        /// Creates a new <see cref="EmailAddress"/> using the specified email address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns>a new <see cref="EmailAddress"/> address or null</returns>
        public static EmailAddress TryCreate(string address)
        {
            var valid = Validate.EmailAddress(address);
            if (valid == null)
                return null;

            return new EmailAddress(valid, true);
        }

        /// <summary>
        /// Implicitly converts a string to an <see cref="EmailAddress"/>.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static implicit operator EmailAddress(string address)
        {
            return new EmailAddress(address);
        }

        private static string GetLocalPart(string valid)
        {
            var indexOfSign = valid.IndexOf('@');
            return valid.Substring(0, indexOfSign);
        }

        private static string GetDomainPart(string valid)
        {
            var indexOfSign = valid.IndexOf('@');
            return valid.Substring(indexOfSign + 1);
        }

        private static string GetAccountPart(string local)
        {
            var indexOfSign = local.IndexOf('+');
            if (indexOfSign >= 0)
                return local.Substring(0, indexOfSign);
            else
                return local;
        }

        private static string GetTagPart(string local)
        {
            var indexOfSign = local.IndexOf('+');
            if (indexOfSign >= 0)
                return local.Substring(indexOfSign + 1);
            else
                return null;
        }

        /// <summary>
        /// Returns the validated lower-cased address.
        /// </summary>
        public string Value
        {
            get { return this.address; }
        }

        /// <summary>
        /// Returns the validated lower-cased address without the tag.
        /// </summary>
        public string ValueWithoutTag
        {
            get { return this.accountPart + "@" + this.domainPart; }
        }

        /// <summary>
        /// Returns the local part (everything before @).
        /// </summary>
        public string LocalPart
        {
            get { return this.localPart; }
        }

        /// <summary>
        /// Returns the domain part (everything after @).
        /// </summary>
        public string DomainPart
        {
            get { return this.domainPart; }
        }

        /// <summary>
        /// Gets the account part (everything before @ or before the first +).
        /// </summary>
        public string AccountPart
        {
            get { return this.accountPart; }
        }

        /// <summary>
        /// Gets the tag part (everything before @ and after the first +).
        /// </summary>
        public string TagPart
        {
            get { return this.tagPart; }
        }

        /// <summary>
        /// Gets the original string that was passed to the <see cref="EmailAddress"/> constructor.
        /// </summary>
        public string OriginalString
        {
            get { return this.original; }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.address;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(EmailAddress other)
        {
            if ((object)other == (object)null)
                return false;

            return string.Equals(other.Value, this.Value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is EmailAddress))
                return false;

            return this.Equals((EmailAddress)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static bool operator ==(EmailAddress left, EmailAddress right)
        {
            if ((object)left == (object)null && (object)right == (object)null)
                return true;

            if ((object)left == (object)null)
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(EmailAddress left, EmailAddress right)
        {
            if ((object)left == (object)null && (object)right == (object)null)
                return false;

            if ((object)left == (object)null)
                return true;

            return !left.Equals(right);
        }
    }
}
