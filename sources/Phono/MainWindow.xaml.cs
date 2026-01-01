using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Phono.Helpers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Phono.Contracts.Services.App;
using System.Diagnostics;
using System.Threading.Tasks;


namespace Phono
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public static new MainWindow Current => _current ??= new MainWindow();
        private static MainWindow _current;

        private TaskCompletionSource<bool> _rootFrameReadyTcs = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public MainWindow()
        {
            InitializeComponent();
            EnsureWindow();

            // Try to register immediately in case XAML name resolution already happened
            TryRegisterRootFrame();

            // RootPageHost is defined in XAML; wait for it to be loaded before registering frame as a fallback
            if (RootPageHost != null)
            {
                RootPageHost.Loaded += MainWindow_Loaded;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TryRegisterRootFrame();
            }
            catch (Exception ex)
            {
                // Log the error instead of swallowing it so registration failures can be diagnosed
                Debug.WriteLine($"MainWindow_Loaded: Failed to register root frame: {ex}");
            }
        }

        private void TryRegisterRootFrame()
        {
            var nav = Locator.Instance.GetService<INavigationService>();
            if (nav == null)
            {
                return;
            }

            if (RootPageHost != null)
            {
                var obj = RootPageHost.FindName("RootFrame");
                var rootFrame = obj as Frame;
                if (rootFrame != null)
                {
                    try
                    {
                        nav.RegisterForFrame(rootFrame, Contracts.Services.App.TargetFrameOption.RootFrame);
                        // Signal any awaiters that root frame is ready
                        _rootFrameReadyTcs.TrySetResult(true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"TryRegisterRootFrame: RegisterForFrame threw: {ex}");
                    }
                }
            }
        }

        /// <summary>
        /// Returns a task which completes when the root frame has been registered.
        /// </summary>
        public Task WaitForRootFrameAsync()
        {
            return _rootFrameReadyTcs.Task;
        }

        private void EnsureWindow()
        {
            WindowHelper.TrackWindow(this);
            var appWindow = AppWindow;
            if (appWindow != null)
            {
                appWindow.Title = "Phono";// TO-DO : Localize
                this.ExtendsContentIntoTitleBar = true;
                appWindow.TitleBar.IconShowOptions = IconShowOptions.HideIconAndSystemMenu;
                appWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            }
        }
        
    }
}
