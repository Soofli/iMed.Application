using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaManager;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iMed.App.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoPopUp : ContentView
    {
        public VideoPopUp(string videoUrl)
        {
            InitializeComponent();
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @$"<html><body>
                                <video height=""100%"" width=""100%"" controls autoplay controlsList=""nodownload"">
                                <source src = ""{Addresses.DownloadVideo}/{videoUrl}?access_token={UtilitiesWrapper.Instance.UserUtilities.AccessToken.access_token}"" type = ""video/mp4"">
                                </video>
                                </body></html>";
            webView.Source = htmlSource;
            //MediaElement.Source = MediaSource.FromUri($"{Addresses.DownloadVideo}/{videoUrl}?access_token={UtilitiesWrapper.Instance.UserUtilities.AccessToken.access_token}");
            //MediaElement.MediaOpened += (sender, args) =>
            //{
            //    indicatorFrame.IsVisible = false;
            //};
            MessagingCenter.Send(this, "SetLandscapeModeOn");
        }
        private async void CloseButton_OnClicked(object sender, EventArgs e)
        {
            await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
            MessagingCenter.Send(this, "SetLandscapeModeOff");
        }

        private void FullScreenButton_OnClicked(object sender, EventArgs e)
        {
            if (MediaElement.Aspect == Aspect.AspectFill)
                MediaElement.Aspect = Aspect.AspectFit;
            else if(MediaElement.Aspect == Aspect.Fill)
                MediaElement.Aspect = Aspect.AspectFill;
            else if (MediaElement.Aspect == Aspect.AspectFit)
                MediaElement.Aspect = Aspect.Fill;

        }

        private void PlaySpeedButton_OnClicked(object sender, EventArgs e)
        {
            //MediaElement.Speed += 2;
        }
    }
}