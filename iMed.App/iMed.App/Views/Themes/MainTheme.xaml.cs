using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iMed.App.Views.Themes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTheme : ResourceDictionary
    {
        public MainTheme()
        {
            InitializeComponent();
        }
    }
}