using Xamarin.Forms;

namespace iMed.App.Views.Pages
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        private void Telegram_OnTapped(object sender, EventArgs e)
        {
            if (BindingContext is ProfilePageViewModel viewModel)
                viewModel.TelegramCommand.Execute(null);
        }

        private void Whatsapp_OnTapped(object sender, EventArgs e)
        {

            if (BindingContext is ProfilePageViewModel viewModel)
                viewModel.WhatsAppCommand.Execute(null);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ProfilePageViewModel viewModel)
                viewModel.Initialize(null);
        }
    }
}
