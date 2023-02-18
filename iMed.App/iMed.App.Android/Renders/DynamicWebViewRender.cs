using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using iMed.App.Droid.Renders;
using iMed.App.Renders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WebView = Android.Webkit.WebView;

[assembly: ExportRenderer(typeof(DynamicWebView), typeof(DynamicWebViewRender))]
namespace iMed.App.Droid.Renders
{

    public class DynamicWebViewRender : WebViewRenderer
    {
        public static int _webViewHeight;
        static DynamicWebView _xwebView = null;
        Android.Webkit.WebView _webView;
        public DynamicWebViewRender(Context context) : base(context)
        {

        }
        public override bool DispatchTouchEvent(MotionEvent e)
        {
            Parent.RequestDisallowInterceptTouchEvent(true);
            return base.DispatchTouchEvent(e);
        }

        public class CustomWebViewClient : Android.Webkit.WebViewClient
        {
            Android.Webkit.WebView _webView;
            public override async void OnPageFinished(Android.Webkit.WebView view, string url)
            {
                try
                {
                    _webView = view;
                    if (_xwebView != null)
                    {

                        view.Settings.JavaScriptEnabled = true;
                        //view.Settings.UseWideViewPort = true;
                        await Task.Delay(500);
                        var result = await _xwebView.EvaluateJavaScriptAsync(@"document.getElementById(""mainDiv"").offsetHeight;");
                        var top = double.Parse(await _xwebView.EvaluateJavaScriptAsync(@"document.querySelector('#mainDiv').getBoundingClientRect().top"));
                        var bot = double.Parse(await _xwebView.EvaluateJavaScriptAsync(@"document.querySelector('#mainDiv').getBoundingClientRect().bottom"));
                        var size = ((bot / 10) - top) + 60;
                        if (size < 80)
                            size = 180;
                        _xwebView.HeightRequest = size;
                        _xwebView.WebViewSize();
                    }
                    base.OnPageFinished(view, url);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);
            _xwebView = e.NewElement as DynamicWebView;

            _webView = Control;

            if (e.OldElement == null)
            {
                _webView.SetWebViewClient(new CustomWebViewClient());
            }

        }
    }
}