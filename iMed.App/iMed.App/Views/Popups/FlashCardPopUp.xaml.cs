using iMed.Domain.Dtos.LargDtos;
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
    public partial class FlashCardPopUp : ContentView
    {
        public FlashCardPopUp(FlashCardSDto flashCard)
        {
            InitializeComponent();
            tagLabel.Text = flashCard.FlashCardTagName;
            questionLabel.Text = flashCard.Question;
            longAnswerLabel.Text = flashCard.LongAnswer;
        }
        public FlashCardPopUp(UserFlashCardStatusLDto flashCard)
        {
            InitializeComponent();
            categoryLabel.Text = flashCard.FlashCardCategoryName;
            tagLabel.Text = flashCard.FlashCardTagName;
            questionLabel.Text = flashCard.Question;
            longAnswerLabel.Text = flashCard.LongAnswer;
            typeLabel.Text = flashCard.FlashCardType.ToDisplay();
        }

        private async void CloseButton_Clicked(object sender, EventArgs e)
        {
            await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
        }
    }
}