using System;
using System.Net.Http;
using System.Reflection;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using FFImageLoading;
using FFImageLoading.Config;
using FFImageLoading.Forms.Platform;
using iMed.App.Utilities;
using iMed.App.Views.Pages;
using iMed.App.Views.Popups;
using MediaManager;
using MediaManager.Forms.Platforms.Android;
using Prism;
using Prism.Ioc;
using Rg.Plugins.Popup;
using Xamarin.Android.Net;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace iMed.App.Droid
{
    [Activity(Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize,LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;
                Popup.Init(this);
                CachedImageRenderer.Init(false);
                base.OnCreate(savedInstanceState);

                Forms.Init(this, savedInstanceState);
                LoadApplication(new App(new AndroidInitializer()));
                CrossMediaManager.Current.Init();
                Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    Window.DecorView.SystemUiVisibility = StatusBarVisibility.Hidden;
                    var statusBarHeightInfo = typeof(FormsAppCompatActivity).GetField("_statusBarHeight", BindingFlags.Instance | BindingFlags.NonPublic);
                    statusBarHeightInfo?.SetValue(this, 0);
                }
                //Window?.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);
                CrossMediaManager.Current.Init();
                RequestedOrientation = ScreenOrientation.Portrait;
                SetHttpClientForFFImage();
                SetLandscapeModeMassageCenter();
            }
            catch (Exception e)
            {
                Log.Error("iMed", e.Message);
            }
        }

        private void SetHttpClientForFFImage()
        {
            var client = new HttpClient(new AndroidClientHandler());
            ImageService.Instance.Initialize(new Configuration
            {
                HttpClient = client
            });
        }

        private void SetLandscapeModeMassageCenter()
        {
            MessagingCenter.Subscribe<VideoPopUp>(this, "SetLandscapeModeOn", sender =>
            {
                RequestedOrientation = ScreenOrientation.Landscape;
                Window?.AddFlags(WindowManagerFlags.Fullscreen);
            });
            MessagingCenter.Subscribe<VideoPopUp>(this, "SetLandscapeModeOff", sender =>
            {
                RequestedOrientation = ScreenOrientation.Portrait;
                Window?.ClearFlags(WindowManagerFlags.Fullscreen);
            });
            MessagingCenter.Subscribe<PopUpUtilities>(this, "SetLandscapeModeOff", sender =>
            {
                RequestedOrientation = ScreenOrientation.Portrait;
                Window?.ClearFlags(WindowManagerFlags.Fullscreen);
            });
        }

        public override void OnBackPressed()
            => Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

