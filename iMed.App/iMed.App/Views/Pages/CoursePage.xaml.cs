using Xamarin.Forms;

namespace iMed.App.Views.Pages
{
    public partial class CoursePage : ContentPage
    {
        public CoursePage()
        {
            InitializeComponent();
            mainTabView.SelectedIndex = 1;
        }

        private void PurchaseTapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            if(BindingContext is CoursePageViewModel viewModel)
                viewModel.PurchaseCourseCommand.Execute(null);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Send(this, "SetLandscapeModeOn");
        }
    }
}
