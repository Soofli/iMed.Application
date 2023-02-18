using iMed.App.Views.Popups;

namespace iMed.App.ViewModels.Pages;
public class ProfilePageViewModel : ViewModelBase<ProfilePageDto>
{
    public ICommand IncreaseInventoryCommand { get; set; }
    public ICommand PurchasesCommand { get; set; }
    public ICommand EnableEditCommand { get; set; }
    public ICommand DisableEditCommand { get; set; }
    public ICommand EditUserCommand { get; set; }
    public ICommand LogOutCommand { get; set; }
    public ICommand UploadIdentityImageCommand { get; set; }
    public ICommand TelegramCommand { get; set; }
    public ICommand WhatsAppCommand { get; set; }
    public ImageSource IdentityImageSource { get; set; }
    public bool EditIsEnable { get; set; } = false;
    public bool ForceEditIsEnable { get; set; } = false;
    public bool IsRunning { get; set; }
    public ProfilePageViewModel(INavigationService navigationService, IRestWrapper restWrapper) : base(navigationService, restWrapper)
    {
    }

    public override void InitializeCommand()
    {
        base.InitializeCommand();
        LogOutCommand = new DelegateCommand(async () =>
        {
            var popUp = new QuestionPopUp("آیا می خواهید از حساب کاربردی خود خارج شوید ؟");
            popUp.Accepted += async (sender, args) =>
            {

                Preferences.Remove(PreferencesNames.AccessToken);
                await NavigationService.NavigateAsync("myapp:///NavigationPage/AuthenticationPage");
            };
            await UtilitiesWrapper.Instance.PopUpUtilities.PushAsync(popUp);
        });
        UploadIdentityImageCommand = new DelegateCommand(async () =>
        {
            try
            {
                UtilitiesWrapper.Instance.PopUpUtilities.PushIndicator();

                var file = await FilePicker.PickAsync(new PickOptions { PickerTitle = "لطفا عکس کارت هویت خود را انتخاب کنید" });
                if (file != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        var stream = await file.OpenReadAsync();
                        await stream.CopyToAsync(memoryStream);
                        IdentityImageSource = ImageSource.FromStream(() => stream);
                        await RestWrapper.FileRestApi.UploadIdentityImage(new FileUploadRequest
                        {
                            FileUploadType = FileUploadType.Image,
                            FileName = file.FileName,
                            StringBaseFile = Convert.ToBase64String(memoryStream.ToArray())
                        }, UtilitiesWrapper.Instance.BearerToken);
                    }
                }
                await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
                UtilitiesWrapper.Instance.PopUpUtilities.PushSuccess("تغییر اطلاعات کاربری با موفقیت انجام شد");
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
        EditUserCommand = new DelegateCommand(async () =>
        {
            try
            {
                UtilitiesWrapper.Instance.PopUpUtilities.PushIndicator();

                if (PageDto.User.FirstName.IsNullOrEmpty())
                    throw new AppException("نام خود را وارد کنید");
                if (PageDto.User.LastName.IsNullOrEmpty())
                    throw new AppException("نام خانوادگی خود را وارد کنید");
                if (PageDto.User.PhoneNumber.IsNullOrEmpty())
                    throw new AppException("شماره تلفن خود را وارد کنید");
                if (PageDto.User.DtoPassword == "******")
                    PageDto.User.DtoPassword = null;
                await RestWrapper.AccountRestApi.EditUser(PageDto.User, UtilitiesWrapper.Instance.BearerToken);
                await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
                UtilitiesWrapper.Instance.PopUpUtilities.PushSuccess("تغییر اطلاعات کاربری با موفقیت انجام شد");
                EditIsEnable = false;
                ForceEditIsEnable = false;
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
        EnableEditCommand = new DelegateCommand(() =>
        {
            ForceEditIsEnable = true;
            if(!PageDto.User.IsConfirmed)
                EditIsEnable = true;
        });
        DisableEditCommand = new DelegateCommand(() =>
        {
            ForceEditIsEnable = false;
            EditIsEnable = false;
        });
        IncreaseInventoryCommand = new DelegateCommand(async () =>
        {
            var popUp = new IncreaseInventoryPopUp();
            await UtilitiesWrapper.Instance.PopUpUtilities.PushAsync(popUp);
        });
        PurchasesCommand = new DelegateCommand(async () =>
        {

            await NavigationService.NavigateAsync("PurchasesPage");
        });
        TelegramCommand = new DelegateCommand(async () => await Browser.OpenAsync("https://t.me/Boardgameshiraz"));
        WhatsAppCommand = new DelegateCommand(async () => await Browser.OpenAsync("https://wa.me/+989106859732"));
    }

    public override async void Initialize(INavigationParameters parameters)
    {
        base.Initialize(parameters);
        try
        {
            if (IsRunning)
                return;
            IsRunning = true;
            UtilitiesWrapper.Instance.PopUpUtilities.PushIndicator();
            var rest = await RestWrapper.PageRestApi.ProfilePageAsync(UtilitiesWrapper.Instance.BearerToken);
            PageDto = rest.Data;
            if (!PageDto.User.UserIdentityImageFileName.IsNullOrEmpty())
                IdentityImageSource = ImageSource.FromUri(new Uri($"{Addresses.DownloadIdentityImage}/{PageDto.User.UserIdentityImageFileName}"));
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
        catch (Exception e)
        {
            UtilitiesWrapper.Instance.PopUpUtilities.PushError(e.Message);
        }
        finally
        {
            IsRunning = false;
        }
    }
}
