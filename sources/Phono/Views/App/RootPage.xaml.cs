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


namespace Phono.Views.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RootPage : RootPageBase
    {
        public RootPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var appWindow = MainWindow.Current.AppWindow;
            var titleBar = appWindow.TitleBar;
            titleBar.IconShowOptions = IconShowOptions.HideIconAndSystemMenu;
            titleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            titleBar.PreferredTheme = TitleBarTheme.UseDefaultAppMode;
        }
    }
}
