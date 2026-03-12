using CommunityToolkit.Mvvm.ComponentModel;

namespace PumpsControl.ViewModels;

public partial class DataVisualizationViewModel : ViewModelBase
{
    [ObservableProperty] private double[] _dataX = { 1, 2, 3, 4, 5 };
    [ObservableProperty] private double[] _dataY = { 1, 2, 3, 4, 5 };
}