using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Phono.Contracts.ViewModels;
using System;


namespace Phono.Contracts.Views
{
    public class ScopedAppPageBase<TViewModel> : Page, IDisposable
        where TViewModel : class, IViewModel, IScopedViewModel
    {
        public static DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(TViewModel),
                typeof(ScopedAppPageBase<TViewModel>), new PropertyMetadata(default(TViewModel)));
        private bool disposedValue;

        public TViewModel ViewModel
        {
            get => (TViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public ScopedAppPageBase()
        {
            // Delay resolving scoped page view models until Loaded to ensure proper scope creation
            Loaded += ScopedAppPageBase_Loaded;
            Unloaded += ScopedAppPageBase_Unloaded;
        }

        private void ScopedAppPageBase_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
            {
                try
                {
                    ViewModel = Locator.Instance.GetService<TViewModel>();
                    DataContext = ViewModel;
                }
                catch
                {
                    // swallow to avoid breaking UI initialization; failing cases should be logged if needed
                }
            }
        }

        private void ScopedAppPageBase_Unloaded(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            ViewModel = null;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Dispose();
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

        ~ScopedAppPageBase()
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

    public class SingletonAppPageBase<TViewModel> : Page, IDisposable
        where TViewModel : class, IViewModel, ISingletonViewModel
    {
        public static DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(TViewModel),
                typeof(SingletonAppPageBase<TViewModel>), new PropertyMetadata(default(TViewModel)));
        private bool disposedValue;

        public TViewModel ViewModel
        {
            get => (TViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public SingletonAppPageBase()
        {
            // Delay resolving singleton app page view models until Loaded
            Loaded += SingletonAppPageBase_Loaded;
            Unloaded += SingletonAppPageBase_Unloaded;
        }

        private void SingletonAppPageBase_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
            {
                try
                {
                    ViewModel = Locator.Instance.GetService<TViewModel>();
                    DataContext = ViewModel;
                }
                catch
                {
                    // swallow to avoid breaking UI initialization; failing cases should be logged if needed
                }
            }
        }

        private void SingletonAppPageBase_Unloaded(object sender, RoutedEventArgs e)
        {
            DataContext = null;
            ViewModel = null;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Dispose();
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

        ~SingletonAppPageBase()
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
