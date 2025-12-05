using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Phono.Contracts.ViewModels;


namespace Phono.Contracts.Views
{
    public class AppPageBase<TViewModel> : Page
        where TViewModel : class, IViewModel
    {
        public static DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(TViewModel),
                typeof(AppPageBase<TViewModel>), new PropertyMetadata(default(TViewModel)));

        public TViewModel ViewModel
        {
            get => (TViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public AppPageBase()
        {
            ViewModel = Locator.Instance.GetService<TViewModel>();
            DataContext = ViewModel;
        }
    }
}
