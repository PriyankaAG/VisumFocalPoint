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
    [Activity(Label = "Focal Point", Icon = "@drawable/icon", Theme = "@style/Theme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode)]
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
            DeviceOrientation();
        }
        string id = string.Empty;

        private void DeviceOrientation()
        {
            //allowing the device to change the screen orientation based on the rotation
            MessagingCenter.Subscribe<FocalPoint.Modules.FrontCounter.Views.OrderSignatureView>(this, "allowLandScapePortrait", sender => { RequestedOrientation = ScreenOrientation.Unspecified; });
            MessagingCenter.Subscribe<FocalPoint.Modules.FrontCounter.Views.SignatureTermsView>(this, "allowLandScapePortrait", sender => { RequestedOrientation = ScreenOrientation.Unspecified; });

            //during page close setting back to portrait
            /*MessagingCenter.Subscribe<FocalPoint.Modules.FrontCounter.Views.OrderSignatureView>(this, "RestoreOrientation", sender =>
            {
                if (App.ForceLandscape == true)
                    RequestedOrientation = ScreenOrientation.Landscape;
                else
                    RequestedOrientation = ScreenOrientation.Portrait;
            });*/
            /* MessagingCenter.Subscribe<FocalPoint.Pages.SignatureTermsPage>(this, "RestoreOrientation", sender =>
             {
                 if (App.ForceLandscape == true)
                     RequestedOrientation = ScreenOrientation.Landscape;
                 else
                     RequestedOrientation = ScreenOrientation.Portrait;
             });*/

            MessagingCenter.Subscribe<FocalPoint.Modules.FrontCounter.Views.OrderSignatureView>(this, "AllowLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Landscape;
            });
            MessagingCenter.Subscribe<FocalPoint.Modules.FrontCounter.Views.SignatureTermsView>(this, "AllowLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Landscape;
            });

            //forces app to portrait mode after closing a Page containing only a Plot
            MessagingCenter.Subscribe<FocalPoint.Modules.FrontCounter.Views.OrderSignatureView>(this, "PreventLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            });
            MessagingCenter.Subscribe<FocalPoint.Modules.FrontCounter.Views.SignatureTermsView>(this, "PreventLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            });
        }
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