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
    public partial class RulesAndPrivacyPopUp : ContentView
    {
        public RulesAndPrivacyPopUp()
        {
            InitializeComponent();
        }

        private void AcceptButton_OnClicked(object sender, EventArgs e)
        {
            UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
        }
    }
}