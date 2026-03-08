using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

    #endregion

    public ControlViewModel(
        ConnectionStore connectionStore,
        ConnectionService connectionService)
    {
        _connectionStore = connectionStore;
        _connectionService = connectionService;
        FrequencyValue = string.Empty;
        
        _connectionStore.PropertyChanged += OnConnectionStoreChanged;
    }

    [ObservableProperty] private string _frequencyValue;

    partial void OnFrequencyValueChanged(string value)
    {
        Debug.WriteLine(value);
    }

    public float CurrentFrequencyValue => _connectionService.ReadData;

    private bool IsConnected => _connectionStore.IsConnected;
    private bool CanControlPump() => IsConnected;

    private void OnConnectionStoreChanged(object? sender, PropertyChangedEventArgs eventArgs)
    {
        if (eventArgs.PropertyName == nameof(IsConnected))
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