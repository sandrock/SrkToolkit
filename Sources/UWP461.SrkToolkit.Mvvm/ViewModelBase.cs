
namespace SrkToolkit.Mvvm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Windows.UI.Xaml.Controls;

    public partial class ViewModelBase
    {
        public void NavigateToPage(Type pageType)
        {
        }

        public NavigationService NavigationService { get; set; }
    }
}
