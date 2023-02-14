using Android.App;
using Android.Content.PM;
using Avalonia.Android;

namespace Tech2023.Native.Android;
[Activity(Label = "Tech2023.Native.Android", Theme = "@style/MyTheme.NoActionBar", Icon = "@drawable/icon", LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
public class MainActivity : AvaloniaMainActivity
{
}
