using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using PumpsControl.ViewModels;

namespace PumpsControl.Services;

public partial class NavigationService(IServiceProvider serviceProvider) : ViewModelBase, INavigationService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [ObservableProperty] private ViewModelBase? _currentView;

    public void NavigateTo<T>() where T : ViewModelBase
    {
        CurrentView = _serviceProvider.GetRequiredService<T>();
    }
}