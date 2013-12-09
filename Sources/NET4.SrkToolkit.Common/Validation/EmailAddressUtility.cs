
namespace SrkToolkit.Common.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents an email address.
    /// </summary>
    public class EmailAddress
    {
        private readonly string original;
        private readonly string address;
        private readonly string localPart;
        private readonly string domainPart;
        private readonly string accountPart;
        private readonly string tagPart;

        /// <summary>
        /// Creates a new <see cref="EmailAddress"/> using the specified email address.
        /// </summary>
        /// <param name="address"></param>
        /// <exception cref="ArgumentException">argument is invalid</exception>
        public EmailAddress(string address)
            : this(address, false)
        {
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
        /// Returns the validated lower-cased address.
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
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.address;
        }
    }
}
