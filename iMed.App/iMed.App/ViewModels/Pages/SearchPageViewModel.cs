namespace iMed.App.ViewModels.Pages;

public class SearchPageViewModel : ViewModelBase<SearchPageDto>
{
    public string SearchText { get; set; }
    public ICommand SelectVideoCommand { get; set; }
    public ICommand SearchCommand { get; set; }
    public SearchPageViewModel(INavigationService navigationService) : base(navigationService)
    {
    }

    public SearchPageViewModel(INavigationService navigationService, IRestWrapper restWrapper) : base(navigationService, restWrapper)
    {
    }

    public override void InitializeCommand()
    {
        base.InitializeCommand();

        SelectVideoCommand = new DelegateCommand<CourseSDto>(async (course) =>
        {
            var param = new NavigationParameters();
            param.Add("Course", course);
            await NavigationService.NavigateAsync("CoursePage", param);
        });
        SearchCommand = new DelegateCommand<string>(async search =>
        {
            SearchText = search;
            try
            {
                UtilitiesWrapper.Instance.PopUpUtilities.PushIndicator();
                var rest = await RestWrapper.PageRestApi.SearchPageAsync(SearchText, UtilitiesWrapper.Instance.BearerToken);
                PageDto = rest.Data;
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
        });
    }

    public override async void Initialize(INavigationParameters parameters)
    {
        base.Initialize(parameters);
        var search = parameters.GetValue<string>("Search");
        SearchText = search;
        try
        {
            UtilitiesWrapper.Instance.PopUpUtilities.PushIndicator();
            var rest = await RestWrapper.PageRestApi.SearchPageAsync(SearchText,UtilitiesWrapper.Instance.BearerToken);
            PageDto = rest.Data;
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
    }
}