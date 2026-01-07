using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Phono.Models.App;
using Richasy.WinUIKernel.Share.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Management.Deployment.Preview;

namespace Phono.Helpers
{
    public class NavigationViewHelper
    {
        public static NavigationViewItemModel GetItem<TPage>(string name, FluentIcons.Common.Symbol icon)
            where TPage : class
        {
            return new NavigationViewItemModel()
            {
                Title = name,
                Symbol = icon,
                PageId = typeof(TPage).Name
            };

        }

        public static string GetNavigateTo(NavigationViewItem item) => (string)item.GetValue(NavigateToProperty);

        public static void SetNavigateTo(NavigationViewItem item, string value) => item.SetValue(NavigateToProperty, value);

        public static readonly DependencyProperty NavigateToProperty =
        DependencyProperty.RegisterAttached("NavigateTo", typeof(Page), typeof(NavigationViewHelper), new PropertyMetadata(null));
    }
}
