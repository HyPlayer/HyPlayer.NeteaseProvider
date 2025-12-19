using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Phono.Contracts.ViewModels;
using System;


namespace Phono.Contracts.Views
{
    public class DialogBase<TViewModel> : ContentDialog, IDisposable
        where TViewModel : class, IViewModel
    {
        public static DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(TViewModel),
                typeof(AppPageBase<TViewModel>), new PropertyMetadata(default(TViewModel)));
        private bool disposedValue;

        public TViewModel ViewModel
        {
            get => (TViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public DialogBase()
        {
            ViewModel = Locator.Instance.GetService<TViewModel>();
            DataContext = ViewModel;
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
