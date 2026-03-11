using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using PumpsControl.Services;

namespace PumpsControl.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        _navigationService.NavigateTo<ConnectionViewModel>();
    }

    [ObservableProperty] private bool _enableSplitView;


    [ObservableProperty] private INavigationService _navigationService;

    [RelayCommand]
    private void NavigateToConnectionSettings()
    {
        NavigationService.NavigateTo<ConnectionViewModel>();
    }

    [RelayCommand]
    private void NavigateToMain()
    {
        NavigationService.NavigateTo<MainViewModel>();
    }

    [RelayCommand]
    private void NavigateToControl()
    {
        NavigationService.NavigateTo<ControlViewModel>();
    }

    [RelayCommand]
    private void ToggleSplitView()
    {
        EnableSplitView = !EnableSplitView;
    }
}