

namespace iMed.App.Views.Popups.Originals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class SuccessPopUp : ContentView
    {
        public SuccessPopUp()
        {
            InitializeComponent();
            Timer timer = new Timer(3000);
            timer.Start();
            timer.Elapsed += (async (sender, e) =>
            {
                await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
                timer.Close();
            });
        }
        public SuccessPopUp(string message)
        {
            InitializeComponent();
            messageLabel.Text = message;
            Timer timer = new Timer(3000);
            timer.Start();
            timer.Elapsed += (async (sender, e) =>
            {
                await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
                timer.Close();
            });
        }
    }
}