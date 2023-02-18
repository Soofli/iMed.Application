namespace iMed.App.Views.Popups;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class IncreaseInventoryPopUp : ContentView
{
    private RestWrapper RestWrapper { get; } = new RestWrapper();
    public event EventHandler IncreaseInventoryDone;
    public string Amount { get; set; }
    public IncreaseInventoryPopUp()
    {
        BindingContext = this;
        InitializeComponent();
    }

    private async void CancelButton_OnClicked(object sender, EventArgs e)
    {
        await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
    }

    private async void SubmitButton_OnClicked(object sender, EventArgs e)
    {
        try
        {
            UtilitiesWrapper.Instance.PopUpUtilities.PushIndicator();
            Amount = amountEntry.Text.ConvertToOrginal();
            if (!int.TryParse(Amount, out int amount))
                throw new AppException("مقدار وارد شده صحیح نمی باشد");
            var rest = await RestWrapper.WalletRestApi.IncreaseInventory(amount, UtilitiesWrapper.Instance.BearerToken);
            if (rest.Data.NeedToPayment)
                await Browser.OpenAsync(rest.Data.PaymentUrl);
            else
                UtilitiesWrapper.Instance.PopUpUtilities.PushSuccess("حساب شما افزایش پیدا کرد");
            await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
        }
        catch (ApiException apiException)
        {
            var exe = await apiException.GetContentAsAsync<ApiResult>();
            if (exe != null)
                UtilitiesWrapper.Instance.PopUpUtilities.PushError(exe.Message);
            else
                UtilitiesWrapper.Instance.PopUpUtilities.PushError("در اجرای درخواست شما مشکلی پیش امده است . 711");

        }
        catch (Exception ex)
        {
            UtilitiesWrapper.Instance.PopUpUtilities.PushError(ex.Message);
        }
    }

    private void AmountEntry_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            var converted = entry.Text.ConvertTo3Digit();
            if (entry.Text.Equals(converted) == false)
            {
                entry.Text = converted;
                entry.CursorPosition = entry.Text.Length;
                entry.SelectionLength = 0;
            }
        }
    }
}