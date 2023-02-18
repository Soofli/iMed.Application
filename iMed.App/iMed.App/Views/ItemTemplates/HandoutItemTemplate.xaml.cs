using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iMed.App.Views.ItemTemplates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HandoutItemTemplate : ContentView
    {
        public static readonly BindableProperty ShowCommandProperty =
            BindableProperty.Create(
                propertyName: nameof(ShowCommand),
                returnType: typeof(ICommand),
                declaringType: typeof(HandoutItemTemplate),
                defaultValue: null,
                defaultBindingMode: BindingMode.OneWay
            );

        public ICommand ShowCommand
        {
            get { return (ICommand)GetValue(ShowCommandProperty); }
            set { SetValue(ShowCommandProperty, value); }
        }

        public static readonly BindableProperty PurchaseCommandProperty =
            BindableProperty.Create(
                propertyName: nameof(PurchaseCommand),
                returnType: typeof(ICommand),
                declaringType: typeof(HandoutItemTemplate),
                defaultValue: null,
                defaultBindingMode: BindingMode.OneWay
            );

        public ICommand PurchaseCommand
        {
            get { return (ICommand)GetValue(PurchaseCommandProperty); }
            set { SetValue(PurchaseCommandProperty, value); }
        }
        public HandoutItemTemplate()
        {
            InitializeComponent();
        }

        private void PurchaseTapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            if (PurchaseCommand != null)
                PurchaseCommand.Execute(this.BindingContext);
        }

        private void PlayTapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            if (ShowCommand != null)
                ShowCommand.Execute(this.BindingContext);
        }
    }
}