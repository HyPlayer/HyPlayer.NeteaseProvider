using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Phono.Contracts.ViewModels;
using System;


namespace Phono.Contracts.Views
{
    public class ControlBase<TViewModel> : UserControl, IDisposable
        where TViewModel : class, IViewModel
    {
        public static DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(TViewModel),
                typeof(ControlBase<TViewModel>), new PropertyMetadata(default(TViewModel)));
        private bool disposedValue;

        public TViewModel ViewModel
        {
            get => (TViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public ControlBase()
        {
            // Delay resolving ViewModel until control is loaded to avoid resolving scoped services during XAML construction
            Loaded += ControlBase_Loaded;
            Unloaded += ControlBase_Unloaded;
        }

        private void ControlBase_Loaded(object sender, RoutedEventArgs e)
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

        private void ControlBase_Unloaded(object sender, RoutedEventArgs e)
        {
            // Optionally clear DataContext when unloaded to break references
            DataContext = null;
            ViewModel = null;
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

        ~ControlBase()
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
