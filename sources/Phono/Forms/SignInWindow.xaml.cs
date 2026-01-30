using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Richasy.WinUIKernel.Share;
using Richasy.WinUIKernel.Share.Base;
using WinUIEx;
using Microsoft.UI;
using Phono.ViewModels.Netease;
using AsyncAwaitBestPractices;

namespace Phono.Forms
{
    public sealed partial class SignInWindow : WindowBase
    {
        private SignInViewModel ViewModel => Locator.Instance.GetService<SignInViewModel>();

        public SignInWindow()
        {
            InitializeComponent();
            this.CenterOnScreen();

            var titleBar = AppWindow.TitleBar;
            titleBar.ExtendsContentIntoTitleBar = true;
            titleBar.BackgroundColor = Colors.Transparent;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonHoverBackgroundColor = Colors.Transparent;
            titleBar.ButtonPressedBackgroundColor = Colors.Transparent;

            SetTitleBar(TitleBarArea);
        }

        private void ImageEx_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadDataAsync(QRCodePresenter).SafeFireAndForget();
        }       
    }
}
