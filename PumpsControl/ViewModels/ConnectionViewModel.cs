using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PumpsControl.ViewModels;

public partial class ConnectionViewModel : ViewModelBase
{
    [ObservableProperty]
    private List<string> _cpuType = new List<string>()
    {
        "S71200",
        "S71500",
        "LogoOBA8"
    };
}