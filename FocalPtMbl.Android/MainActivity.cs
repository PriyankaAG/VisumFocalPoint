using System;
using FocalPoint;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using FocalPtMbl.Droid;
using FocalPtMbl.Themes;
using Xamarin.Forms;
using static Android.Provider.Settings;
using FocalPoint.MainMenu.Services;
using System.Threading.Tasks;
using System.IO;

namespace FocalPtMbl.Droid
{
    [Activity(Label = "Focal Point 3", Icon = "@drawable/icon", Theme = "@style/Theme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = FocalPtMbl.Droid.Resource.Layout.Tabbar;
           // ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Xamarin.Forms.Platform.Android.init
            ThemeLoaderImplementation themeLoader = DependencyService.Get<IThemeLoader>() as ThemeLoaderImplementation;
            if (themeLoader != null)
                themeLoader.Activity = this;

            Environment_Android environment = DependencyService.Get<FocalPtMbl.MainMenu.Views.IEnvironment>() as Environment_Android;
            if (environment != null)
                environment.Activity = this;

            DependencyService.RegisterSingleton<IDeviceInfo>(new DeviceInfo());
            CreateAndLoadApplication(Intent);
        }
        string id = string.Empty;

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            App application = Xamarin.Forms.Application.Current as FocalPtMbl.App;
            // application?.ProcessNotificationIfNeed(intent.GetReminderId(), intent.GetRecurrenceIndex());
        }
        void CreateAndLoadApplication(Intent intent)
        {
            App app = new App();
            // app.ProcessNotificationIfNeed(intent.GetReminderId(), intent.GetRecurrenceIndex());
            LoadApplication(app);

        }
        internal void UpdateNightMode(bool isLightTheme)
        {
           // AppCompatDelegate.DefaultNightMode = isLightTheme ? AppCompatDelegate.ModeNightNo : AppCompatDelegate.ModeNightYes;
           // Delegate.ApplyDayNight();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        // Field, property, and method for Picture Picker
        public static readonly int PickImageId = 1000;

        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);

                    // Set the Stream as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(stream);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }
    }
}