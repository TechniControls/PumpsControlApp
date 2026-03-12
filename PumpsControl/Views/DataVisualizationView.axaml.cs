using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PumpsControl.ViewModels;
using ScottPlot.Avalonia;

namespace PumpsControl.Views;

public partial class DataVisualizationView : UserControl
{
    public DataVisualizationView()
    {
        InitializeComponent();

        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is DataVisualizationViewModel vm)
        {
            AvaPlot avaPlot = this.Find<AvaPlot>("AvaPlot") ?? throw new InvalidOperationException();
            avaPlot.Plot.Add.DataLogger().Add(vm.DataX, vm.DataY);
            avaPlot?.Refresh();
        }
    }
}