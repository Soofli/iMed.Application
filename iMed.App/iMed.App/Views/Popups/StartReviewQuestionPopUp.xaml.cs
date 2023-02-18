using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iMed.App.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartReviewQuestionPopUp : ContentView
    {
        public event EventHandler NewFlashCardSelected;
        public event EventHandler OldFlashCardSelected;
        public event EventHandler AllFlashCardSelected;
        public StartReviewQuestionPopUp()
        {
            InitializeComponent();
        }

        private async void NewFlashCardsButton_Clicked(object sender, EventArgs e)
        {
            NewFlashCardSelected?.Invoke(this, EventArgs.Empty);
            await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
        }
        private async void OldFlashCardsButton_Clicked(object sender, EventArgs e)
        {
            OldFlashCardSelected?.Invoke(this, EventArgs.Empty);
            await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
        }
        private async void AllFlashCardsButton_Clicked(object sender, EventArgs e)
        {
            AllFlashCardSelected?.Invoke(this, EventArgs.Empty);
            await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
        }
    }
}