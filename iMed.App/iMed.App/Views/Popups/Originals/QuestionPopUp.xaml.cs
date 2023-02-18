using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMed.App.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iMed.App.Views.Popups.Originals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionPopUp : ContentView
    {
        public event EventHandler Accepted;
        public event EventHandler Canceled;
        public QuestionPopUp()
        {
            InitializeComponent();
        }
        public QuestionPopUp(string message)
        {
            InitializeComponent();
            messageLabel.Text = message;
        }

        private async void AcceptButton_OnClicked(object sender, EventArgs e)
        {
            Accepted?.Invoke(this, new EventArgs());
            await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
        }

        private async void CancelButton_OnClicked(object sender, EventArgs e)
        {
            Canceled?.Invoke(this, new EventArgs());
            await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
        }
    }
}