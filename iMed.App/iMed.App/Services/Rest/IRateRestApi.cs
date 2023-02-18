namespace iMed.App.Services.Rest;

public interface IRateRestApi
{
    [Get("/CourseRate/{courseId}")]
    Task<ApiResult<List<CourseRateSDto>>> GetCourseRate(int courseId, [Query] int page, [Header("Authorization")] string authorization);

    [Post("/CourseRate")]
    Task<ApiResult> PostCourseRate([Body] CourseRateSDto courseRate, [Header("Authorization")] string authorization);
    [Post("/FlashCardCategoryRate")]
    Task<ApiResult> PostFlashCardCategoryRate([Body] FlashCardCategoryRate flashCardCategoryRate, [Header("Authorization")] string authorization);
}