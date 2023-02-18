namespace iMed.App.ViewModels;
public class MainPageViewModel : ViewModelBase<MainPageDto>
{
    public ICommand ProfileCommand { get; set; }
    public ICommand SelectSpecialOfferCommand { get; set; }
    public ICommand SelectVideoCommand { get; set; }
    public ICommand SelectFlashCardCommand { get; set; }
    public ICommand LeitnerBoxCommand { get; set; }
    public ICommand SearchCommand { get; set; }
    public bool IsRunning { get; set; }
    public MainPageViewModel(INavigationService navigationService,IRestWrapper restWrapper) : base(navigationService,restWrapper)
    {
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
            var rest = await RestWrapper.PageRestApi.MainPageAsync(UtilitiesWrapper.Instance.BearerToken);
            PageDto = rest.Data;
            await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
        }
        catch (ApiException apiException)
        {
            
            var exe = await apiException.GetContentAsAsync<ApiResult>();
            if (exe != null)
                UtilitiesWrapper.Instance.PopUpUtilities.PushError(exe.Message);
            else
            {
                UtilitiesWrapper.Instance.PopUpUtilities.PushError("در اجرای درخواست شما مشکلی پیش امده است . 711");
                Preferences.Remove(PreferencesNames.AccessToken);
                await NavigationService.NavigateAsync("myapp:///NavigationPage/AuthenticationPage");
            }

        }
        catch (Exception e)
        {
            if(e.Message.Contains("Unable to resolve host"))
            {
                Preferences.Remove(PreferencesNames.AccessToken);
                await NavigationService.NavigateAsync("myapp:///NavigationPage/AuthenticationPage");
            }
            else
                UtilitiesWrapper.Instance.PopUpUtilities.PushError(e.Message);

        }
        finally
        {
            IsRunning = false;
        }
    }

    public override void InitializeCommand()
    {
        base.InitializeCommand();
        LeitnerBoxCommand = new DelegateCommand(async () => await NavigationService.NavigateAsync("LeitnerBoxPage"));
        SelectFlashCardCommand = new DelegateCommand<FlashCardCategorySDto>(async dto =>
        {

            var param = new NavigationParameters();
            param.Add("FlashCardCategory", dto);
            await NavigationService.NavigateAsync("FlashCardCategoryPage", param);
        });
        SearchCommand = new DelegateCommand<string>(async search =>
        {
            try
            {
                var param = new NavigationParameters();
                if (search.IsNullOrEmpty())
                    throw new AppException("لطفا متن جستجو خود را وارد کنید");
                param.Add("Search", search);
                await NavigationService.NavigateAsync("SearchPage", param);
            }
            catch (Exception e)
            {
                UtilitiesWrapper.Instance.PopUpUtilities.PushError(e.Message);
            }
        });
        SelectVideoCommand = new DelegateCommand<CourseSDto>(async (course) =>
        {
            var param = new NavigationParameters();
            param.Add("Course", course);
            await NavigationService.NavigateAsync("CoursePage", param);
        });
        ProfileCommand = new DelegateCommand(async () =>
        {
            await NavigationService.NavigateAsync("ProfilePage");
        });
        SelectSpecialOfferCommand = new DelegateCommand<SpecialOfferSDto>(async offer =>
        {
            if (offer.SpecialOfferType == SpecialOfferType.Course)
            {
                var param = new NavigationParameters();
                param.Add("Course", offer.Course);
                await NavigationService.NavigateAsync("CoursePage", param);
            }
            else if (offer.SpecialOfferType == SpecialOfferType.FlashCard)
            {
                var param = new NavigationParameters();
                param.Add("FlashCardCategory", offer.FlashCardCategory);
                await NavigationService.NavigateAsync("FlashCardCategoryPage", param);
            }
        });
    }
}
