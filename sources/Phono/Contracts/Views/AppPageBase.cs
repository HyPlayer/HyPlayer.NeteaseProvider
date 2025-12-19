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
            ViewModel = Locator.Instance.GetService<TViewModel>();
            DataContext = ViewModel;
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
            ViewModel = Locator.Instance.GetService<TViewModel>();
            DataContext = ViewModel;
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
