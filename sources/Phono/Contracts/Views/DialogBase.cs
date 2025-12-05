using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Phono.Contracts.ViewModels;


namespace Phono.Contracts.Views
{
    public class DialogBase<TViewModel> : ContentDialog
        where TViewModel : class, IViewModel
    {
        public static DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(TViewModel),
                typeof(AppPageBase<TViewModel>), new PropertyMetadata(default(TViewModel)));

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
    }
}
