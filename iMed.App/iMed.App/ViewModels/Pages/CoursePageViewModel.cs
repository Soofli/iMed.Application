namespace iMed.App.ViewModels.Pages;
public class CoursePageViewModel : ViewModelBase<CoursePageDto>
{
    public ICommand AddRateCommand { get; set; }
    public ICommand PurchaseCourseCommand { get; set; }
    public ICommand PlayVideoCommand { get; set; }
    public ICommand PlayFreeVideoCommand { get; set; }
    public ICommand ShowHandoutCommand { get; set; }
    public bool HasFreeVideo { get; set; } = false;
    public ObservableCollection<CourseRateSDto> Rates { get; set; } = new ObservableCollection<CourseRateSDto>();
    public bool PurchaseVisible { get; set; }
    public bool NotPurchaseVisible { get; set; }
    public int RatesCount { get; set; }
    public CoursePageViewModel(INavigationService navigationService) : base(navigationService)
    {
    }

    public CoursePageViewModel(INavigationService navigationService, IRestWrapper restWrapper) : base(navigationService, restWrapper)
    {

    }

    public override void InitializeCommand()
    {
        base.InitializeCommand();
        AddRateCommand = new DelegateCommand(async () =>
        {
            var popUp = new RateActionPopUp(PageDto.Course);
            popUp.Rated += (sender, args) =>
            {
                if(sender is CourseRateSDto rate)
                    Rates.Add(rate);
            };
            await UtilitiesWrapper.Instance.PopUpUtilities.PushAsync(popUp);

        });
        PurchaseCourseCommand = new DelegateCommand(async () =>
        {
            QuestionPopUp questionPopUp = new QuestionPopUp("ایا از خرید این دوره مطمئن هستید ؟");
            questionPopUp.Accepted += async (sender, args) =>
            {
                try
                {
                    UtilitiesWrapper.Instance.PopUpUtilities.PushIndicator();
                    var rest = await RestWrapper.PurchaseRestApi.PurchaseCourse(PageDto.Course.CourseId,
                        UtilitiesWrapper.Instance.BearerToken);
                    await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
                    UtilitiesWrapper.Instance.PopUpUtilities.PushSuccess("دوره مورد نظر خریداری شد");
                    NotPurchaseVisible = false;
                    PurchaseVisible = true;
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
            };
            await UtilitiesWrapper.Instance.PopUpUtilities.PushAsync(questionPopUp);
        });
        PlayVideoCommand = new DelegateCommand<VideoSDto>(async (video) =>
        {
            await UtilitiesWrapper.Instance.PopUpUtilities.PushAsync(new VideoPopUp(video.FileName),false);
            
        });
        PlayFreeVideoCommand = new DelegateCommand(async () =>
        {
            var firstFree = PageDto.Videos.FirstOrDefault(v => v.IsFree);
            if(firstFree!=null)
                await UtilitiesWrapper.Instance.PopUpUtilities.PushAsync(new VideoPopUp(firstFree.FileName), false);

        });
        ShowHandoutCommand = new DelegateCommand<CourseHandoutSDto>(async (hadnout) =>
        {
            var address = $"{Addresses.DownloadHandout}/{hadnout.FileName}";
            await Browser.OpenAsync(address,new BrowserLaunchOptions
            {
                TitleMode = BrowserTitleMode.Hide
            });
        });
    }

    public override async void Initialize(INavigationParameters parameters)
    {
        base.Initialize(parameters);
        var course = parameters.GetValue<CourseSDto>("Course");
        PageDto.Course = course;
        try
        {
            UtilitiesWrapper.Instance.PopUpUtilities.PushIndicator();
            var rates = await RestWrapper.RateRestApi.GetCourseRate(PageDto.Course.CourseId, 1, UtilitiesWrapper.Instance.BearerToken);
            var page = await RestWrapper.PageRestApi.GetCoursePage(PageDto.Course.CourseId, UtilitiesWrapper.Instance.BearerToken);

            PageDto.IsPurchased = page.Data.IsPurchased;
            PageDto.CourseTime = page.Data.CourseTime;
            PageDto.RateAvg = page.Data.RateAvg;
            PageDto.RateCount = page.Data.RateCount;
            PurchaseVisible = PageDto.IsPurchased;
            NotPurchaseVisible = !PageDto.IsPurchased;
            RatesCount = rates.Data.Count;
            page.Data.Videos.OrderBy(v=>v.Row).ForEach(c =>
            {
                c.IsPurchase = !c.IsFree ? PageDto.IsPurchased : c.IsFree;
                PageDto.Videos.Add(c);
            });
            page.Data.Handouts.ForEach(h =>
            {
                h.IsPurchase = PageDto.IsPurchased;
                PageDto.Handouts.Add(h);
            });
            rates.Data.OrderBy(r=>r.CreatedAt).ForEach(r => Rates.Add(r));
            if (PageDto.Videos != null && PageDto.Videos.Any(v => v.IsFree))
                HasFreeVideo = true;

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
