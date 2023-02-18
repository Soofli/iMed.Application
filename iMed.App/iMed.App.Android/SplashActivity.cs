using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace iMed.App.Droid
{
    [Activity(Theme = "@style/MainTheme.Splash",
        Name = "com.vnf.imed.SplashActivity",
              MainLauncher = true,
              NoHistory = true)]
    [IntentFilter(new[] { "schemas.com.vnf.imed.SplashActivity" }, Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable, Android.Content.Intent.CategoryAlternative })]
    public class SplashActivity : AppCompatActivity
    {
        private ImageView logo;
        private TextView versionTextView;
        // Launches the startup task
        protected async override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SplashActivityLayout);
            logo = FindViewById<ImageView>(Resource.Id.logo);
            versionTextView = FindViewById<TextView>(Resource.Id.versionTextView);
            if (versionTextView != null)
                versionTextView.Text = Application.Context?.ApplicationContext?.PackageManager?
                    .GetPackageInfo(Application.Context?.ApplicationContext?.PackageName, 0)?.VersionName;
            await Task.Delay(TimeSpan.FromSeconds(1));
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

        protected async override void OnResume()
        {
            base.OnResume();
            logo = FindViewById<ImageView>(Resource.Id.logo);
            versionTextView = FindViewById<TextView>(Resource.Id.versionTextView);
            if (versionTextView != null)
                versionTextView.Text = Application.Context?.ApplicationContext?.PackageManager?
                    .GetPackageInfo(Application.Context?.ApplicationContext?.PackageName, 0)?.VersionName;
            await Task.Delay(TimeSpan.FromSeconds(1));
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));

        }
    }
}
