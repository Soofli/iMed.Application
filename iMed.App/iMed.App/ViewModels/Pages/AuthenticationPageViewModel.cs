namespace iMed.App.ViewModels.Pages;
public class AuthenticationPageViewModel : ViewModelBase
{
    public UiItemModel<string> PhoneNumber { get; set; } = new UiItemModel<string> { ErrorMessage = "لطفا شماره تلفن را به درستی وارد کنید" };
    public UiItemModel<string> Password { get; set; } = new UiItemModel<string> { ErrorMessage = "لطفا پسورد صحیح را وارد کنید" };
    public UiItemModel<string> FirstName { get; set; } = new UiItemModel<string> { ErrorMessage = "لطفا نام خود را به درستی وارد کنید" };
    public UiItemModel<string> LastName { get; set; } = new UiItemModel<string> { ErrorMessage = "لطفا نام خانوادگی را به درستی وارد کنید" };
    public UiItemModel<string> VerifyCode { get; set; } = new UiItemModel<string> { ErrorMessage = "لطفا کد اعتبارسنجی را وارد کنید" };
    public bool VerifyPhoneVisible { get; set; } = true;
    public bool SignUpVisible { get; set; } = false;
    public bool LoginVisible { get; set; } = false;
    public ICommand VerifyPhoneCommand { get; set; }
    public ICommand SignUpCommand { get; set; }
    public ICommand RulesAndPrivacyCommand { get; set; }
    public ICommand LoginCommand { get; set; }
    public ICommand ForgetPasswordCommand { get; set; }
    public ICommand BackCommand { get; set; }
    public AuthenticationPageViewModel(INavigationService navigationService, IRestWrapper restWrapper)
        : base(navigationService, restWrapper)
    {

    }

    public override void InitializeCommand()
    {
        base.InitializeCommand();
        VerifyPhoneCommand = new DelegateCommand(async () =>
        {
            try
            {
                if (!new Regex(@"(^(989|0989|\+989|09|9)[0-9]{9}$)").IsMatch(PhoneNumber.Value))
                    PhoneNumber.HasError = true;
                else
                    PhoneNumber.HasError = false;

                if (PhoneNumber.HasError)
                    throw new AppException("لطفا شماره تلفن را به درستی وارد کنید");
                if (PhoneNumber.Value.IsNullOrEmpty())
                    throw new AppException("شماره تلفن را وارد کنید");
                UtilitiesWrapper.Instance.PopUpUtilities.PushIndicator();
                var rest = await RestWrapper.AuthorizeRestApi.VerifyPhoneNumberAsync(PhoneNumber.Value);
                await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
                VerifyPhoneVisible = false;
                if (rest.Data)
                    LoginVisible = true;
                else
                    SignUpVisible = true;
            }
            catch (ApiException apiException)
            {
                await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
                PhoneNumber.HasError = true;
                var exe = await apiException.GetContentAsAsync<ApiResult>();
                if (exe != null)
                    PhoneNumber.ErrorMessage = exe.Message;
                else
                    PhoneNumber.ErrorMessage = "در اجرای درخواست شما مشکلی پیش امده است . 711"+" "+apiException.Content+" "+apiException.StatusCode.ToString();

            }
            catch (Exception e)
            {
                await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
                PhoneNumber.HasError = true;
                PhoneNumber.ErrorMessage = e.Message;
            }
        });
        RulesAndPrivacyCommand = new DelegateCommand(async () =>
        {
            await UtilitiesWrapper.Instance.PopUpUtilities.PushAsync(new RulesAndPrivacyPopUp(), false);
        });
        SignUpCommand = new DelegateCommand(async () =>
        {
            try
            {
                if (Password.Value.IsNullOrEmpty())
                {
                    Password.HasError = true;
                    return;
                }
                if (LastName.Value.IsNullOrEmpty())
                {
                    LastName.HasError = true;
                    return;
                }
                if (FirstName.Value.IsNullOrEmpty())
                {
                    FirstName.HasError = true;
                    return;
                }
                if (VerifyCode.Value.IsNullOrEmpty())
                {
                    VerifyCode.HasError = true;
                    return;
                }

                Password.HasError = false;
                LastName.HasError = false;
                FirstName.HasError = false;
                VerifyCode.HasError = false;

                UtilitiesWrapper.Instance.PopUpUtilities.PushIndicator();
                var rest = await RestWrapper.AuthorizeRestApi.SignUpUserAsync(new SignUpRequestDto
                {
                    Password = Password.Value,
                    FirstName = FirstName.Value,
                    LastName = LastName.Value,
                    Phone = PhoneNumber.Value,
                    UserName = PhoneNumber.Value,
                    VerifyCode = VerifyCode.Value
                });
                await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
                Preferences.Set(PreferencesNames.AccessToken, JsonConvert.SerializeObject(rest.Data));
                UtilitiesWrapper.Instance.UserUtilities.AccessToken = rest.Data;
                await NavigationService.NavigateAsync("myapp:///NavigationPage/MainPage");

            }
            catch (ApiException apiException)
            {
                var exe = await apiException.GetContentAsAsync<ApiResult>();
                if (exe != null)
                    UtilitiesWrapper.Instance.PopUpUtilities.PushError(exe.Message);
                else
                    UtilitiesWrapper.Instance.PopUpUtilities.PushError("در اجرای درخواست شما مشکلی پیش امده است . 711");

            }
            catch (Exception e)
            {
                UtilitiesWrapper.Instance.PopUpUtilities.PushError(e.Message);
            }
        });
        LoginCommand = new DelegateCommand(async () =>
        {
            try
            {
                if (Password.Value.IsNullOrEmpty())
                {
                    Password.HasError = true;
                    return;
                }
                Password.HasError = false;

                UtilitiesWrapper.Instance.PopUpUtilities.PushIndicator();
                var rest = await RestWrapper.AuthorizeRestApi.LoginUserAsync(new LoginRequestDto
                {
                    Password = Password.Value,
                    UserName = PhoneNumber.Value,
                });
                await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
                Preferences.Set(PreferencesNames.AccessToken, JsonConvert.SerializeObject(rest.Data));
                UtilitiesWrapper.Instance.UserUtilities.AccessToken = rest.Data;
                var res = await NavigationService.NavigateAsync("myapp:///NavigationPage/MainPage");

            }
            catch (ApiException apiException)
            {
                var exe = await apiException.GetContentAsAsync<ApiResult>();
                if (exe != null)
                    UtilitiesWrapper.Instance.PopUpUtilities.PushError(exe.Message);
                else
                    UtilitiesWrapper.Instance.PopUpUtilities.PushError("در اجرای درخواست شما مشکلی پیش امده است . 711");

            }
            catch (Exception e)
            {
                UtilitiesWrapper.Instance.PopUpUtilities.PushError(e.Message);
            }
        });
        ForgetPasswordCommand = new DelegateCommand(async() =>
        {
            await UtilitiesWrapper.Instance.PopUpUtilities.PushAsync(new ForgetPasswordPopUp(PhoneNumber.Value));
        });
        BackCommand = new DelegateCommand(() =>
        {
            LoginVisible = false;
            SignUpVisible = false;
            VerifyPhoneVisible = true;
        });
    }
}
