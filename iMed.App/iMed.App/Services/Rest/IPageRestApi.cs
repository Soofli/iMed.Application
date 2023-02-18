namespace iMed.App.Services.Rest;

public interface IPageRestApi
{

    [Get("/App/MainPage")]
    Task<ApiResult<MainPageDto>> MainPageAsync([Header("Authorization")] string token);

    [Get("/App/PurchasesPage")]
    Task<ApiResult<PurchasePageDto>> PurchasesPageAsync([Header("Authorization")] string token);

    [Get("/App/SearchPage")]
    Task<ApiResult<SearchPageDto>> SearchPageAsync([Query]string search,[Header("Authorization")] string token);

    [Get("/App/profilepage")]
    Task<ApiResult<ProfilePageDto>> ProfilePageAsync([Header("Authorization")] string token);

    [Get("/App/CoursePage/{courseId}")]
    Task<ApiResult<CoursePageDto>> GetCoursePage(int courseId, [Header("Authorization")] string token);

    [Get("/App/FlashCardCategoryPage/{flashCardCategoryId}")]
    Task<ApiResult<FlashCardCategoryPageDto>> GetFlashCardCategoryPage(int flashCardCategoryId, [Header("Authorization")] string token);

    [Get("/App/LeitnerBoxPage")]
    Task<ApiResult<LeitnerBoxPageDto>> GetLeitnerBoxPage( [Header("Authorization")] string token);
}