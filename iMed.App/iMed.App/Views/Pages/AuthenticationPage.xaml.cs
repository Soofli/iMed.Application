
using ValueChangedEventArgs = Syncfusion.XForms.MaskedEdit.ValueChangedEventArgs;

namespace iMed.App.Views.Pages;
public partial class AuthenticationPage : ContentPage
{
    public AuthenticationPage()
    {
        InitializeComponent();
    }

    private void SfMaskedEdit_OnValueChanged(object sender, ValueChangedEventArgs eventargs)
    {
        SfMaskedEdit maskedEdit = sender as SfMaskedEdit;
        if (maskedEdit.HasError)
        {
            DisplayAlert("Alert", "Please enter valid details", "OK");
        }
    }
}
