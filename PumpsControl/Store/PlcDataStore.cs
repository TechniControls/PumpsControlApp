using CommunityToolkit.Mvvm.ComponentModel;
using PumpsControl.ViewModels;

namespace PumpsControl.Store;

public partial class PlcDataStore : ViewModelBase
{
    [ObservableProperty] private float _currentFrequency;
}