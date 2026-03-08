using System;
using CommunityToolkit.Mvvm.ComponentModel;
using PumpsControl.Services;
using PumpsControl.ViewModels;

namespace PumpsControl.Store;

public partial class ConnectionStore() : ViewModelBase
{
    
    [ObservableProperty] private string _selectedCpu = string.Empty;
    [ObservableProperty] private string _ipAddress = string.Empty;
    [ObservableProperty] private Int16 _plcRack = 0;
    [ObservableProperty] private Int16 _plcSlot = 0;
    [ObservableProperty] private bool _isConnected;
}