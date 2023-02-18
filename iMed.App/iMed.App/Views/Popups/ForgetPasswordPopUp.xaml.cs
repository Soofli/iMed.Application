namespace iMed.App.Views.Popups;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ForgetPasswordPopUp : ContentView
{
    private RestWrapper _restWrapper = new RestWrapper();
    public ForgetPasswordPopUp()
    {
        InitializeComponent();
    }

    public ForgetPasswordPopUp(string phoneNumber)
    {
        InitializeComponent();
        phoneEntry.Text = phoneNumber;
        phoneEntry.IsReadOnly = true;
    }
    private async void SubmitButton_OnClicked(object sender, EventArgs e)
    {
        try
        {
            UtilitiesWrapper.Instance.PopUpUtilities.PushIndicator();
            await _restWrapper.AuthorizeRestApi.ForgetPasswordAsync(phoneEntry.Text);
            await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
            UtilitiesWrapper.Instance.PopUpUtilities.PushSuccess("رمز عبور جدید برای شما ارسال خواهد شد");
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

    private async void CancelButton_OnClicked(object sender, EventArgs e)
    {
        await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
    }
}