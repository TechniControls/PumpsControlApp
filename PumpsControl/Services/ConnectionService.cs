using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MsBox.Avalonia;
using PumpsControl.Store;
using PumpsControl.ViewModels;
using S7.Net;

namespace PumpsControl.Services;

public partial class ConnectionService(ConnectionStore connectionStore) : ViewModelBase
{
    private readonly ConnectionStore _connectionStore = connectionStore;

    #region PLC Access

    private Plc? _plcStation;
    private CancellationTokenSource? _cancellationToken;

    #endregion

    [ObservableProperty] private float _readData;

    #region Methods for Connect and Disconnect PLC

    public async Task<bool> AsyncConnectPlc(string cpuType, string ipAddress, Int16 plcRack, Int16 plcSlot)
    {
        if (string.IsNullOrWhiteSpace(cpuType) || string.IsNullOrWhiteSpace(ipAddress) || plcSlot <= 0)
        {
            var messageBox =
                MessageBoxManager.GetMessageBoxStandard("Invalid Data",
                    "The fields cannot be empty or PLC Slot cannot be 0");
            await messageBox.ShowAsync();
        }

        try
        {
            CpuType selectedCpuType = cpuType switch
            {
                "S71200" => CpuType.S71200,
                "S71500" => CpuType.S71500,
                "LogoOBA8" => CpuType.Logo0BA8,
                _ => CpuType.S71200
            };

            // Create Plc Station and open async connection
            _plcStation = new Plc(selectedCpuType, ipAddress, plcRack, plcSlot);
            await _plcStation.OpenAsync();

            _connectionStore.IsConnected = true;


            return true;
        }
        catch (Exception e)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Connection Error", e.Message);
            await messageBox.ShowAsync();
            return false;
        }
    }


    public async Task<bool> DisconnectPlc()
    {
        try
        {
            if (_plcStation != null && _plcStation is { IsConnected: true })
            {
                _cancellationToken?.Cancel();
                _plcStation.Close();
                _connectionStore.IsConnected = false;
            }

            return true;
        }
        catch (Exception e)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Disconnect Error", $"Error {e}");
            await messageBox.ShowAsync();
            return false;
        }
    }

    #endregion

    #region Methods Write PLC Bits

    // Write Bits
    public async Task WriteBitControlPump<T>(string address, T value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (_plcStation is { IsConnected: true } && _plcStation != null)
        {
            try
            {
                await _plcStation.WriteAsync(address, value);
            }
            catch (Exception e)
            {
                var messageBox = MessageBoxManager.GetMessageBoxStandard("Write Bit Error", $"Error {e}");
                await messageBox.ShowAsync();
            }
        }
    }

    // Write Data
    public async Task WriteDataControlPump<T>(string address, T value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (_plcStation is { IsConnected: true } && _plcStation != null)
        {
            try
            {
                await _plcStation.WriteAsync(address, value);
            }
            catch (Exception e)
            {
                var messageBox = MessageBoxManager.GetMessageBoxStandard("Write Data Error", $"Error {e}");
                await messageBox.ShowAsync();
            }
        }
    }

    #endregion

    #region Read Data

    public async Task ReadDataControlPump(string address)
    {
        if (_plcStation is { IsConnected: true } && _plcStation != null)
        {
            _cancellationToken?.Cancel();
            _cancellationToken = new CancellationTokenSource();
            var token = _cancellationToken.Token;

            _ = Task.Run(async () =>
            {
                while (!_cancellationToken.Token.IsCancellationRequested)
                {
                    try
                    {
                        var readValue = await _plcStation.ReadAsync(DataType.DataBlock,1,2, VarType.Real, 1);
                        ReadData = Convert.ToSingle(readValue);
                        Debug.WriteLine($"Read Data: {ReadData}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    await Task.Delay(5000, token);
                }
            }, token);
        }
    }

    #endregion
}