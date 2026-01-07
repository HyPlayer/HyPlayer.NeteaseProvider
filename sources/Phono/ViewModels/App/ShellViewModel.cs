using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Phono.Contracts.Services;
using Phono.Contracts.Services.App;
using Phono.Contracts.ViewModels;
using Phono.Helpers;
using Phono.Models.App;
using Phono.Views.Netease;
using Phono.Views.Settings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Symbol = FluentIcons.Common.Symbol;


namespace Phono.ViewModels.App
{
    public partial class ShellViewModel :
        ObservableRecipient, IViewModel, ISingletonViewModel
    {
        private readonly INavigationViewService _navigationViewService;
        private readonly INavigationService _navigationService;

        public ObservableCollection<NavigationViewItemModel> NavigationViewItems { get; } = new();
        public ObservableCollection<NavigationViewItemModel> FooterNavigationViewItems { get; } = new();

        public ShellViewModel(INavigationViewService navigationViewService, INavigationService navigationService)
        {
            _navigationViewService = navigationViewService;
            _navigationService = navigationService;
        }

        public void Initialize()
        {
            var footerNavItemsList = new List<NavigationViewItemModel>()
            {
                NavigationViewHelper.GetItem<TestPage>("Settings", Symbol.Settings)
            };
            var navItemsList = new List<NavigationViewItemModel>()
            {
                NavigationViewHelper.GetItem<HomePage>("Home", Symbol.Home),
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

        public void InitializeNavigationView(NavigationView navView, Frame shellFrame)
        {
            if (navView == null || shellFrame == null)
            {
                return;
            }

            _navigationViewService?.Initialize(navView);
            _navigationService?.RegisterForFrame(shellFrame, TargetFrameOption.ShellFrame);
        }
    }
}
