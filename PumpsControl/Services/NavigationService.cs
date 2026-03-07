using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using PumpsControl.ViewModels;

namespace PumpsControl.Services;

public partial class NavigationService : ViewModelBase, INavigationService
{
    private readonly IServiceProvider _serviceProvider;
    
    [ObservableProperty]
    private ViewModelBase? _currentView;
    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void NavigateTo<T>() where T : ViewModelBase
    {
        CurrentView = _serviceProvider.GetRequiredService<T>();
    }
}