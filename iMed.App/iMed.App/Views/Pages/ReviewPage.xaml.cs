using iMed.Domain.Dtos.LargDtos;
using Xamarin.Forms;

namespace iMed.App.Views.Pages
{
    public partial class ReviewPage : ContentPage
    {
        public ReviewPage()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            CarouselView.Position+=1;
        }

        private void PauseTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if(BindingContext is ReviewPageViewModel viewModel)
                viewModel.PauseReviewCommand.Execute(null);
        }
        protected override bool OnBackButtonPressed()
        {
            if (BindingContext is ReviewPageViewModel viewModel)
                viewModel.PauseReviewCommand.Execute(null);
            return true;
        }

        private void Archive_Tapped(object sender, EventArgs e)
        {
            if (BindingContext is ReviewPageViewModel viewModel)
                viewModel.ArchiveFlashCardCommand.Execute(null);
        }
    }
}
