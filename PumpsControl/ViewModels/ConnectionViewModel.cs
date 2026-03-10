using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using PumpsControl.Services;
using PumpsControl.Store;

namespace PumpsControl.ViewModels;

public partial class ConnectionViewModel
    : ViewModelBase
{
    #region DI

    private readonly ConnectionStore _connectionStore;
    private readonly ConnectionService _connectionService;

    #endregion

    public ConnectionViewModel(
        ConnectionStore connectionStore,
        ConnectionService connectionService)
    {
        _connectionService = connectionService;
        _connectionStore = connectionStore;

        // Initialize fields
        SelectedCpu = _connectionStore.SelectedCpu;
        IpAddress = _connectionStore.IpAddress;
        PlcRack = _connectionStore.PlcRack;
        PlcSlot = _connectionStore.PlcSlot;

        _connectionStore.PropertyChanged += OnConnectionStoreChanged;
    }


    #region Properties for Show Connection Data

    [ObservableProperty] private List<string> _cpuType = new List<string>()
    {
        "S71200",
        "S71500",
        "LogoOBA8"
    };

    #endregion

    #region Properties for Capture Selected Data

    // Property for capture selected cpu
    [ObservableProperty] private string _selectedCpu;

    partial void OnSelectedCpuChanged(string value)
    {
        if (_connectionStore.SelectedCpu == value) return;
        _connectionStore.SelectedCpu = value;
        Debug.WriteLine("Selected Cpu: " + value);
    }

    // Property for capture ip address
    [ObservableProperty] private string _ipAddress;

    partial void OnIpAddressChanged(string value)
    {
        if (_connectionStore.IpAddress == value) return;
        _connectionStore.IpAddress = value;
        Debug.WriteLine("IpAddress: " + value);
    }

    // Property for capture plc rack
    [ObservableProperty] private Int16 _plcRack;

    partial void OnPlcRackChanged(Int16 value)
    {
        if (_connectionStore.PlcRack == value) return;
        _connectionStore.PlcRack = value;
        Debug.WriteLine("PlcRack: " + value);
    }

    // Property for capture plc rack
    [ObservableProperty] private Int16 _plcSlot;

    partial void OnPlcSlotChanged(Int16 value)
    {
        if (_connectionStore.PlcSlot == value) return;
        _connectionStore.PlcSlot = value;
        Debug.WriteLine("PlcSlot: " + value);
    }

    #endregion

    #region Properties for Show UI Data

    public string ConnectionStatus => IsConnected ? "Connected" : "Disconnected";
    public IBrush ConnectionStatusColor => IsConnected ? Brushes.Green : Brushes.Orange;

    #endregion

    #region Update Properties when Connection Store Changed

    // Capture connection state from Connection Store when connect plc and disconnect plc
    private bool IsConnected => _connectionStore.IsConnected;


    // When properties IsConnected of connection store is updated, update properties 
    private void OnConnectionStoreChanged(object? sender, PropertyChangedEventArgs eventArgs)
    {
        if (eventArgs.PropertyName == nameof(IsConnected))
        {
            // Connection Status
            OnPropertyChanged(nameof(ConnectionStatus));
            OnPropertyChanged(nameof(ConnectionStatusColor));
            // Update execute RelayCommands
            AsyncConnectPlcCommand.NotifyCanExecuteChanged();
            DisconnectPlcCommand.NotifyCanExecuteChanged();

            // Read Data


            Debug.WriteLine("PlcSlot: " + IsConnected);
        }
    }

    #endregion

    #region Relays Commands

    // Can Execute commands conditional
    private bool CanConnect() => !IsConnected;
    private bool CanDisconnect() => IsConnected;

    // Commands
    // Connect Plc Command
    [RelayCommand(CanExecute = nameof(CanConnect))]
    private async Task AsyncConnectPlc()
    {
        bool result = await _connectionService.AsyncConnectPlc(
            SelectedCpu,
            IpAddress,
            PlcRack,
            PlcSlot);

        if (result)
        {
            var messageBox =
                MessageBoxManager.GetMessageBoxStandard("Connect Success",
                    "PLC Station has been connected successfully");
            await messageBox.ShowAsync();
            
            // Execute read data form plc
            // If connection is successfully, start read data
            await _connectionService.ReadDataControlPump("DB1.DBD2");
        }
    }

    // Disconnect Plc Command
    [RelayCommand(CanExecute = nameof(CanDisconnect))]
    private async Task DisconnectPlc()
    {
        bool result = await _connectionService.DisconnectPlc();
        if (result)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Disconnect Success",
                "PLC Station has been disconnect successfully");
            await messageBox.ShowAsync();
        }
    }

    #endregion
}