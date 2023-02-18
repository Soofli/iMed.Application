using iMed.Domain.Dtos.LargDtos;

namespace iMed.App.Views.Popups;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class RateActionPopUp : ContentView
{
    public event EventHandler Rated;
    public CourseSDto Course { get; }
    public FlashCardCategoryLDto FlashCardCategory { get; }
    public CourseRateSDto CourseRate { get; set; } = new CourseRateSDto();
    public FlashCardCategoryRate FlashCardCategoryRate { get; set; } = new();

    private RestWrapper RestWrapper = new RestWrapper();
    public RateActionPopUp(CourseSDto course)
    {
        Course = course;
        BindingContext = this;
        InitializeComponent();
        nameEntry.Text = UtilitiesWrapper.Instance.UserUtilities.User.FirstName + " " + UtilitiesWrapper.Instance.UserUtilities.User.LastName;
    }
    public RateActionPopUp(FlashCardCategoryLDto flashCardCategory)
    {
        FlashCardCategory = flashCardCategory;
        BindingContext = this;
        InitializeComponent();
        nameEntry.Text = UtilitiesWrapper.Instance.UserUtilities.User.FirstName + " " + UtilitiesWrapper.Instance.UserUtilities.User.LastName;
    }

    private async void CancelButton_OnClicked(object sender, EventArgs e)
    {
        await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
    }

    private async void AcceptButton_OnClicked(object sender, EventArgs e)
    {

        try
        {
            UtilitiesWrapper.Instance.PopUpUtilities.PushIndicator();

            if (nameEntry.Text.IsNullOrEmpty())
                throw new AppException("نام خود را وارد کنید");
            if (messageEditor.Text.IsNullOrEmpty())
                throw new AppException("نظر خود را وارد کنید");
            if (rating.Value < 1)
                throw new AppException("امتیاز ثبتی باید بالاتر از یک باشد");
            if (Course != null)
            {

                CourseRate.CourseId = Course.CourseId;
                CourseRate.Score = (int)rating.Value;
                CourseRate.RateMessage = messageEditor.Text;
                CourseRate.Author = nameEntry.Text;
                await RestWrapper.RateRestApi.PostCourseRate(CourseRate, UtilitiesWrapper.Instance.BearerToken);
                await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
                UtilitiesWrapper.Instance.PopUpUtilities.PushSuccess("نظر شما با موفقیت ثبتــ شد");
                Rated?.Invoke(CourseRate, EventArgs.Empty);
            }
            else
            {
                FlashCardCategoryRate.FlashCardCategoryId = FlashCardCategory.FlashCardCategoryId;
                FlashCardCategoryRate.RateMessage = messageEditor.Text;
                FlashCardCategoryRate.Author = nameEntry.Text;
                FlashCardCategoryRate.Score = (int)rating.Value;
                await RestWrapper.RateRestApi.PostFlashCardCategoryRate(FlashCardCategoryRate, UtilitiesWrapper.Instance.BearerToken);
                await UtilitiesWrapper.Instance.PopUpUtilities.PopAsync();
                UtilitiesWrapper.Instance.PopUpUtilities.PushSuccess("نظر شما با موفقیت ثبتــ شد");
                Rated?.Invoke(FlashCardCategoryRate, EventArgs.Empty);
            }

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
}