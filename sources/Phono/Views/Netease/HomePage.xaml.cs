using AsyncAwaitBestPractices;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Phono.ViewModels.Netease;
using Phono.Views.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace Phono.Views.Netease;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class HomePage : HomePageBase
{
    public HomePage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        /*
        // 如果基类未初始化 ViewModel 或 DataContext，尝试从 Locator 获取并设置（防御式）
        if (DataContext == null)
        {
            try
            {
                var vm = Locator.Instance.GetService<HomeViewModel>();
                if (vm != null)
                {
                    DataContext = vm;
                }
            }
            catch
            {
                // 不抛出，继续让后续代码以安全方式执行
            }
        }*/

        // 安全调用：如果 ViewModel 仍为 null 则不会触发 NRE
        (DataContext as HomeViewModel)?.LoadDataAsync().SafeFireAndForget();
    }

    private void RefreshContainer_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
    {
        ViewModel.LoadDataAsync().SafeFireAndForget();
    }
}
