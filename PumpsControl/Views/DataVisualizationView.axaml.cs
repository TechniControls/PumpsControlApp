using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using PumpsControl.ViewModels;
using ScottPlot.Avalonia;

namespace PumpsControl.Views;

public partial class DataVisualizationView : UserControl
{
    private DataVisualizationViewModel? _dataVisualizationViewModel;
    private AvaPlot _avaPlot;
    private ScottPlot.Plottables.Crosshair _crossHair;


    public DataVisualizationView()
    {
        InitializeComponent();

        _avaPlot = this.Find<AvaPlot>("AvaPlot") ?? throw new InvalidOperationException();

        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        _dataVisualizationViewModel = DataContext as DataVisualizationViewModel;

        if (_dataVisualizationViewModel == null) return;

        InitializePlot();
        _avaPlot.PointerMoved -= AvaPlot_PointerMoved;
        _avaPlot.PointerMoved += AvaPlot_PointerMoved;
    }

    // Initialize and customize plot
    private void InitializePlot()
    {
        // Plot Data
        _avaPlot.Plot.Add.DataLogger().Add(_dataVisualizationViewModel.DataX, _dataVisualizationViewModel.DataY);

        // Crosshair
        _crossHair = _avaPlot.Plot.Add.Crosshair(0, 0);
        _crossHair.TextColor = ScottPlot.Colors.Cyan;
        _crossHair.TextBackgroundColor = ScottPlot.Colors.White;


        _avaPlot?.Refresh();
    }

    private void AvaPlot_PointerMoved(object? sender, PointerEventArgs e)
    {
        var pixelPos = e.GetPosition(_avaPlot);
        var coords = _avaPlot.Plot.GetCoordinates((float)pixelPos.X, (float)pixelPos.Y);

        _crossHair.X = coords.X;
        _crossHair.Y = coords.Y;
        
        _avaPlot.Refresh();
    }
}