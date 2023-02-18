using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using iMed.App.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iMed.App.Views.Popups.Originals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ErrorPopUp : ContentView
    {
        public ErrorPopUp(string message)
        {
            InitializeComponent();
            messageLabel.Text = message;
            Timer timer = new Timer(3000);
            timer.Start();
            timer.Elapsed += ((sender, e) =>
            {
                UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
                timer.Close();
            });
        }
    }
}