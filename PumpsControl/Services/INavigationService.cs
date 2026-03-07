using PumpsControl.ViewModels;

namespace PumpsControl.Services;

public interface INavigationService
{
    ViewModelBase CurrentView { get; }
    void NavigateTo<T>() where  T : ViewModelBase;
}