namespace iMed.App.Services.Rest;

public interface IPurchaseRestApi
{


    [Post("/Course")]
    Task<ApiResult> PurchaseCourse([Query]int courseId, [Header("Authorization")] string authorization);
    [Post("/FlashCardCategory")]
    Task<ApiResult> PurchaseFlashCardCategory([Query] int flashCardCategoryId, [Header("Authorization")] string authorization);
}