using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Telecom;
using Avalonia;
using Avalonia.Android;
using PumpsControl.Store;

namespace PumpsControl.Android;

[Activity(
    Label = "PumpsControl.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
            .WithInterFont();
    }
}