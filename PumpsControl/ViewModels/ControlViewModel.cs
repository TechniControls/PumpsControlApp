using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PumpsControl.Services;
using PumpsControl.Store;

namespace PumpsControl.ViewModels;

public partial class ControlViewModel : ViewModelBase
{
    #region DI

    private readonly ConnectionStore _connectionStore;
    private readonly ConnectionService _connectionService;
    private readonly PlcDataStore _plcDataStore;

    #endregion

    public ControlViewModel(
        ConnectionStore connectionStore,
        ConnectionService connectionService,
        PlcDataStore plcDataStore)
    {
        _connectionStore = connectionStore;
        _connectionService = connectionService;
        _plcDataStore = plcDataStore;
        FrequencyValue = string.Empty;
        
        _plcDataStore.PropertyChanged += OnPlcDataStoreChanged;
    }

    [ObservableProperty]
    private string _frequencyValue;
    public string CurrentFrequencyValue => Convert.ToString(_plcDataStore.CurrentFrequency, CultureInfo.CurrentCulture);

    private bool IsConnected => _connectionStore.IsConnected;
    private bool CanControlPump() => IsConnected;

    private void OnPlcDataStoreChanged(object? sender, PropertyChangedEventArgs eventArgs)
    {
        if (eventArgs.PropertyName == nameof(_plcDataStore.CurrentFrequency))
        {
            OnPropertyChanged(nameof(CurrentFrequencyValue));
        }
    }

    #region Relay Commands

    [RelayCommand(CanExecute = nameof(CanControlPump))]
    private async Task WriteBitStartPump()
    {
        await _connectionService.WriteBitControlPump("DB1.DBX0.0", true);
    }

    [RelayCommand(CanExecute = nameof(CanControlPump))]
    private async Task WriteBitStopPump()
    {
        await _connectionService.WriteBitControlPump("DB1.DBX0.0", false);
    }

    [RelayCommand(CanExecute = nameof(CanControlPump))]
    private async Task WriteDataControlPump()
    {
        float value = float.Parse(FrequencyValue);
        await _connectionService.WriteDataControlPump("DB1.DBD2", value);
    }
    

    #endregion
}