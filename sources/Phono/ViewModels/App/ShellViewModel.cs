using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Phono.Contracts.ViewModels;
using Phono.Helpers;
using Phono.Models.App;
using Phono.Views.Settings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Symbol = FluentIcons.Common.Symbol;


namespace Phono.ViewModels.App
{
    public partial class ShellViewModel :
        ObservableRecipient, IViewModel, ISingletonViewModel
    {
        public ObservableCollection<NavigationViewItemModel> NavigationViewItems { get; } = new();
        public ObservableCollection<NavigationViewItemModel> FooterNavigationViewItems { get; } = new();

        public ShellViewModel()
        {
            
        }

        public void Initialize()
        {
            var footerNavItemsList = new List<NavigationViewItemModel>()
            {
                NavigationViewHelper.GetItem<TestPage>("Settings", Symbol.Settings)
            };
            var navItemsList = new List<NavigationViewItemModel>()
            {
                NavigationViewHelper.GetItem<TestPage>("Home", Symbol.Home),
                NavigationViewHelper.GetItem<TestPage>("Library", Symbol.Library),
            };

            foreach (var item in navItemsList)
            {
                NavigationViewItems.Add(item);
            }

            foreach (var item in footerNavItemsList) 
            {
                FooterNavigationViewItems.Add(item);
            }
        }
    }
}
