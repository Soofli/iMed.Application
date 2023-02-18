using Xamarin.Forms;

namespace iMed.App.Views.Pages
{
    public partial class FlashCardCategoryPage : ContentPage
    {
        public FlashCardCategoryPage()
        {
            InitializeComponent();
        }

        private void PurchaseTapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            if(BindingContext is FlashCardCategoryPageViewModel viewModel)
                viewModel.PurchaseCourseCommand.Execute(null);
        }

        private void LeinterBoxTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (BindingContext is FlashCardCategoryPageViewModel viewModel)
                viewModel.MyLienterBoxCommand.Execute(null);
        }
    }
}
