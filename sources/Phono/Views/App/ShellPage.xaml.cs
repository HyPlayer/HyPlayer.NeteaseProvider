using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Phono.Views.Base;
using Phono.Views.Settings;
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
    public sealed partial class ShellPage : ShellPageBase
    {
        public ShellPage()
        {
            InitializeComponent();
            ShellFrame.Navigate(typeof(TestPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Ensure ViewModel is resolved before using it. Singleton view models are normally
            // resolved in the base class Loaded handler, but OnNavigatedTo can run before Loaded
            // in some navigation scenarios. Try to restore from DataContext or the service locator.
            if (ViewModel == null)
            {
                try
                {
                    ViewModel = DataContext as global::Phono.ViewModels.App.ShellViewModel ?? Locator.Instance.GetService<global::Phono.ViewModels.App.ShellViewModel>();
                    DataContext = ViewModel;
                }
                catch
                {
                    // Swallow here to avoid crashing the navigation path; downstream null checks will protect usage.
                }
            }

            ViewModel?.Initialize();

            // Use the ViewModel API to initialize NavigationView service rather than touching its internal field.
            ViewModel?.InitializeNavigationView(AppNavigationView, ShellFrame);
        }
    }
}
