using System.Threading.Tasks;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using FocalPtMbl.Droid;
using FocalPtMbl.Themes;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.Dependency(typeof(ThemeLoaderImplementation))]
[assembly: Xamarin.Forms.Dependency(typeof(Environment_Android))]

namespace FocalPtMbl.Droid
{
    public class ThemeLoaderImplementation : Java.Lang.Object, IThemeLoader
    {

        public ThemeLoaderImplementation() { }

        public FocalPtMbl.Droid.MainActivity Activity { get; set; }

        public void LoadTheme(ResourceDictionary theme, bool isLightTheme)
        {
            Activity.UpdateNightMode(isLightTheme);
            Android.Graphics.Color backgroundColor = ((Xamarin.Forms.Color)theme["BackgroundThemeColor"]).ToAndroid();
            Device.BeginInvokeOnMainThread(() => {
                Window currentWindow = GetCurrentWindow();
                currentWindow.DecorView.SystemUiVisibility = isLightTheme ? (StatusBarVisibility)SystemUiFlags.LightStatusBar | (StatusBarVisibility)SystemUiFlags.LightNavigationBar : 0;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    currentWindow.SetStatusBarColor(backgroundColor);
                }
                if (Build.VERSION.SdkInt >= BuildVersionCodes.OMr1)
                {
                    currentWindow.SetNavigationBarColor(backgroundColor);
                }
            });

        }
        Window GetCurrentWindow()
        {
            Window window = Activity.Window;
            window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            return window;
        }
    }

    public class Environment_Android : Java.Lang.Object, IEnvironment
    {
        public string PhoneID { get; set; }
        public MainActivity Activity { get; set; }

        public Task<bool> IsLightOperatingSystemTheme()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Froyo)
            {
                UiMode uiModeFlags = Activity.ApplicationContext.Resources.Configuration.UiMode & UiMode.NightMask;
                switch (uiModeFlags)
                {
                    case UiMode.NightYes:
                        return Task.FromResult(false);
                    case UiMode.NightNo:
                        return Task.FromResult(true);
                    default:
                        return Task.FromResult(true);
                }
            }
            else
            {
                return Task.FromResult(true);
            }
        }
    }
}
