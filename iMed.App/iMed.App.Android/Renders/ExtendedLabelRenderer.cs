using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iMed.App.Droid.Renders;
using iMed.App.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRenderer))]
namespace iMed.App.Droid.Renders
{
    
    public class ExtendedLabelRenderer : Xamarin.Forms.Platform.Android.LabelRenderer
    {
        public ExtendedLabelRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Element is ExtendedLabel el)
            {
                if (el.JustifyText)
                {
                    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                        Control.JustificationMode = Android.Text.JustificationMode.InterWord;

                }
            }
        }
    }
}