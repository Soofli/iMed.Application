using iMed.Domain.Dtos.LargDtos;
using Xamarin.Forms;

namespace iMed.App.Views.Pages
{
    public partial class LeitnerBoxPage : ContentPage
    {
        public LeitnerBoxPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (BindingContext is LeitnerBoxPageViewModel viewModel)
                viewModel.Initialize(null);
            base.OnAppearing();
        }

        private void FlashCardTagTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if(sender is Grid { BindingContext : FlashCardCategorySDto flashCardTag } && BindingContext is LeitnerBoxPageViewModel viewModel)
                viewModel.SelectFlashCardTagCommand.Execute(flashCardTag);
        }

        private void LeitnerBoxTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (sender is Grid { BindingContext: LeitnerBoxDto box } && BindingContext is LeitnerBoxPageViewModel viewModel)
                viewModel.SelectLeitnerBoxCommand.Execute(box);
        }

        private void ShowFlashCardButton_Clicked(object sender, EventArgs e)
        {
            if (sender is Button { BindingContext: UserFlashCardStatusLDto flashcard } && BindingContext is LeitnerBoxPageViewModel viewModel)
                viewModel.ShowFlashCardCommand.Execute(flashcard);
        }
    }
}
