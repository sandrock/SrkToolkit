
namespace SrkToolkit.Xaml.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// A base class to select data template based upon 'some' criterium
    /// Taken from http://www.windowsphonegeek.com/articles/Implementing-Windows-Phone-7-DataTemplateSelector-and-CustomDataTemplateSelector
    /// </summary>
    public abstract class DataTemplateSelector : ContentControl
    {
        public virtual DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return null;
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            this.ContentTemplate = this.SelectTemplate(newContent, this);
        }
    }
}
