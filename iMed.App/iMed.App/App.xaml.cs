namespace iMed.App;
public partial class App
{
    public App(IPlatformInitializer initializer) : base(initializer)
    {
    }

    protected override async void OnInitialized()
    {
        SyncfusionLicenseProvider.RegisterLicense("NDgzNUAzMTM4MmUzNDJlMzBFNTFZdHBJQkROTWFMWDQ1dlBoWlNMWERoditkeXlFQVIxcCtuYnc4VTJNPQ==");
        InitializeComponent();
        Initializer.Initialize(false, true);
        var accessTokenJson = Preferences.Get(PreferencesNames.AccessToken, string.Empty);
        if (accessTokenJson.IsNullOrEmpty())
        {
            var res = await NavigationService.NavigateAsync("NavigationPage/AuthenticationPage");
        }
        else
        {
            UtilitiesWrapper.Instance.UserUtilities.AccessToken = JsonConvert.DeserializeObject<AccessToken<UserSDto>>(accessTokenJson);
            //await NavigationService.NavigateAsync("NavigationPage/MainPage");
            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
        containerRegistry.Register<IRestWrapper, RestWrapper>();

        containerRegistry.RegisterForNavigation<NavigationPage>();
        containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
        containerRegistry.RegisterForNavigation<CoursePage, CoursePageViewModel>();
        containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
        containerRegistry.RegisterForNavigation<AuthenticationPage, AuthenticationPageViewModel>();
        containerRegistry.RegisterForNavigation<SearchPage, SearchPageViewModel>();
        containerRegistry.RegisterForNavigation<PurchasesPage, PurchasesPageViewModel>();
        containerRegistry.RegisterForNavigation<VideoPage>();
        containerRegistry.RegisterForNavigation<LeitnerBoxPage, LeitnerBoxPageViewModel>();
        containerRegistry.RegisterForNavigation<ReviewPage, ReviewPageViewModel>();
        containerRegistry.RegisterForNavigation<FlashCardCategoryPage, FlashCardCategoryPageViewModel>();
    }

}
