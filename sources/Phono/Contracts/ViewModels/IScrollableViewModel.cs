namespace Phono.Contracts.ViewModels;

public interface IScrollableViewModel : IViewModel
{
    double? ScrollValue { get; set; }
}

