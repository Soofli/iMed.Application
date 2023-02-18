using System;
using iMed.App.ViewModels;
using Syncfusion.XForms.TabView;
using SelectionChangedEventArgs = Xamarin.Forms.SelectionChangedEventArgs;

namespace iMed.App.Views
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            Timer timer = new Timer(3000);
            timer.Elapsed += (sender, args) =>
            {
                if (BindingContext is MainPageViewModel viewModel)
                    sliderCarousel.Position = (sliderCarousel.Position + 1) % viewModel.PageDto.SpecialOffer.Count;


            };
            timer.Start();
        }

        private void VideoTapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            if (BindingContext is MainPageViewModel viewModel)
                viewModel.SelectVideoCommand.Execute(null);
        }

        protected override void OnAppearing()
        {
            if (BindingContext is MainPageViewModel viewModel)
                viewModel.Initialize(null);
            base.OnAppearing();
        }

        private void LeitnerBoxPageTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (BindingContext is MainPageViewModel viewModel)
                viewModel.LeitnerBoxCommand.Execute(null);
        }
    }
}
