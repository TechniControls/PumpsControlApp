using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using PumpsControl.Services;
using PumpsControl.Store;
using PumpsControl.ViewModels;
using PumpsControl.Views;

namespace PumpsControl;

public partial class App : Application
{
    

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var collection = new ServiceCollection();

        // Services
        collection.AddSingleton<INavigationService, NavigationService>();
        collection.AddSingleton<ConnectionService>();

        // Stores
        collection.AddSingleton<ConnectionStore>();
        collection.AddSingleton<PlcDataStore>();

        // ViewModels
        collection.AddSingleton<MainViewModel>();
        collection.AddTransient<ConnectionViewModel>();
        collection.AddTransient<ControlViewModel>();
        collection.AddTransient<DataVisualizationViewModel>();

        var serviceProvider = collection.BuildServiceProvider();
        var mainViewModel = serviceProvider.GetRequiredService<MainViewModel>();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}