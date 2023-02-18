using DryIoc;
using iMed.App.Renders;
using iMed.App.Views.ItemTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iMed.App.Views.Controllers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LabelUi : ContentView
    {
        public static readonly BindableProperty BGColorProperty =
            BindableProperty.Create(
                propertyName: nameof(BGColor),
                returnType: typeof(Color),
                declaringType: typeof(LabelUi),
                defaultValue: Color.White,
                defaultBindingMode: BindingMode.OneWay,
                null,
                BGColorPropertyChanged
            );

        public Color BGColor
        {
            get { return (Color)GetValue(BGColorProperty); }
            set { SetValue(BGColorProperty, value); }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                propertyName: nameof(Text),
                returnType: typeof(string),
                declaringType: typeof(LabelUi),
                defaultValue: string.Empty,
                defaultBindingMode: BindingMode.OneWay,
                null,
                TextPropertyChanged
            );

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        private static void BGColorPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var viewModel = bindable as LabelUi;
            viewModel.CreateText();
        }
        private static void TextPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var viewModel = bindable as LabelUi;
            viewModel.CreateText();
        }
        private void CreateText()
        {

            string hex;
            if (BGColor.ToHex()?.Count() > 7)
                hex = BGColor.ToHex().Substring(3);
            else
                hex = BGColor.ToHex();
            var htmlText = Text.Replace("\n", "<br>");
            var html = $@"<html>
<head>
<meta charset=""UTF-8"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
<link rel=""preconnect"" href=""//fdn.fontcdn.ir"">
<link rel=""preconnect"" href=""//v1.fontapi.ir"">
<link href=""https://v1.fontapi.ir/css/ShabnamFD"" rel=""stylesheet"">
</head>
<body style=""background-color:#{hex}; overflow : scroll;"">

<div id=""mainDiv"">
<p style=""text-align:justify ;font-family: Shabnam, sans-serif ; direction: rtl;"">
{htmlText}
</p>
</div>

</body>
</html>";
            Task.Delay(300);
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = html;
            var webView = new DynamicWebView { Source = htmlSource };
            webView.WebViewSized += (s, e) =>
            {
                this.HeightRequest = webView.HeightRequest;
            };
            Content = webView;
        }
        public LabelUi()
        {
            InitializeComponent();
        }
    }
}