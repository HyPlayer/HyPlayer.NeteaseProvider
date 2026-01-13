using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Phono.Contracts.ViewModels;
using Phono.Forms;
using Richasy.WinUIKernel.Share;
using Richasy.WinUIKernel.Share.Toolkits;
using System;


namespace Phono.Contracts.Views
{
    public class DialogBase<TViewModel> : ContentDialog, IDisposable
        where TViewModel : class, IViewModel
    {
        public static DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(TViewModel),
                typeof(DialogBase<TViewModel>), new PropertyMetadata(default(TViewModel)));
        private bool disposedValue;

        private readonly IAppToolkit appToolkit;

        public TViewModel ViewModel
        {
            get => (TViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public DialogBase()
        {
            // Defer resolving ViewModel until the control is loaded to ensure any required
            // dependency scope has been created by the caller. Resolving scoped services
            // during construction can fail if there is no active scope.
            this.Loaded += DialogBase_Loaded;

            appToolkit = Locator.Instance.GetService<IAppToolkit>();

            XamlRoot = MainWindow.Current.Content.XamlRoot;
            appToolkit.ResetControlTheme(this);
        }

        private void DialogBase_Loaded(object? sender, RoutedEventArgs e)
        {
            try
            {
                if (ViewModel == null)
                {
                    ViewModel = Locator.Instance.GetService<TViewModel>();
                    DataContext = ViewModel;
                }
            }
            catch
            {
                // Swallow here to avoid crashing the navigation path; downstream null checks will protect usage.
            }
            finally
            {
                this.Loaded -= DialogBase_Loaded;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }

                ViewModel = null;
                disposedValue = true;
            }
        }

        ~DialogBase()
        {

            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
