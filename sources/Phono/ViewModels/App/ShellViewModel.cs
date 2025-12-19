using CommunityToolkit.Mvvm.ComponentModel;
using Phono.Contracts.ViewModels;


namespace Phono.ViewModels.App
{
    public partial class ShellViewModel :
        ObservableRecipient, IViewModel, ISingletonViewModel
    {
        public ShellViewModel()
        {
        }
    }
}
