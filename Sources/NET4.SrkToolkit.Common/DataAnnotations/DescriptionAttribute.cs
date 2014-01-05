// -----------------------------------------------------------------------
// <copyright file="DescriptionAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SrkToolkit.DataAnnotations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false), System.ComponentModel.DataAnnotations.Display]
    public class DescriptionAttribute : Attribute
    {
        public DescriptionAttribute(string Name)
        {
            this.Name = Name;
        }

        public DescriptionAttribute(string ResourceName, Type ResourceType)
        {
            this.ResourceName = ResourceName;
            this.ResourceType = ResourceType;
        }

        public string Name { get; set; }

        public string ResourceName { get; set; }

        public Type ResourceType { get; set; }
    }
}
