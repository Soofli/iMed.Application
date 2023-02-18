namespace iMed.App.ViewModels.Pages;

public class PurchasesPageViewModel : ViewModelBase<PurchasePageDto>
{
    public ICommand SelectVideoCommand { get; set; }
    public PurchasesPageViewModel(INavigationService navigationService, IRestWrapper restWrapper) : base(navigationService, restWrapper)
    {
    }

    public override async void Initialize(INavigationParameters parameters)
    {
        base.Initialize(parameters);
        try
        {
            UtilitiesWrapper.Instance.PopUpUtilities.PushIndicator();
            var rest = await RestWrapper.PageRestApi.PurchasesPageAsync(UtilitiesWrapper.Instance.BearerToken);
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

    public override void InitializeCommand()
    {
        base.InitializeCommand();
        SelectVideoCommand = new DelegateCommand<CourseSDto>(async (course) =>
        {
            var param = new NavigationParameters();
            param.Add("Course", course);
            await NavigationService.NavigateAsync("CoursePage", param);
        });
    }
}