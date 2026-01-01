using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Windowing;
using Phono.Views.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Phono.Services.App;
using Phono.Contracts.Services.App;


namespace Phono.Views.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RootPage : RootPageBase
    {
        private readonly INavigationService _navigationService;

        public RootPage()
        {
            InitializeComponent();
            _navigationService = Locator.Instance.GetService<INavigationService>();

            Loaded += RootPage_Loaded;
            Unloaded += RootPage_Unloaded;
        }

        private void RootPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _navigationService.UnregisterForFrame(TargetFrameOption.RootFrame);
        }

        private void RootPage_Loaded(object sender, RoutedEventArgs e)
        {
            _navigationService.RegisterForFrame(RootFrame, TargetFrameOption.RootFrame);
            var appWindow = MainWindow.Current.AppWindow;
            var titleBar = appWindow.TitleBar;
            titleBar.IconShowOptions = IconShowOptions.HideIconAndSystemMenu;
            titleBar.PreferredTheme = TitleBarTheme.UseDefaultAppMode;
        }
    }
}
