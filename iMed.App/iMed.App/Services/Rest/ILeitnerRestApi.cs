namespace iMed.App.Services.Rest;

public interface ILeitnerRestApi
{
    [Post("/User/Answer/{answerId}")]
    Task<ApiResult<List<SubmitFlashCardAnswerResponseDto>>> SubmitAnswer(int answerId, [Query] double elapsedTime, [Header("Authorization")] string authorization);
    [Post("/User/Answer/Multi")]
    Task<ApiResult<SubmitFlashCardAnswerResponseDto>> SubmitMultiAnswer([Query] int flashCardId, [Body]List<SubmitAnswerRequest> answerRequests, [Header("Authorization")] string authorization);
    [Post("/User/Archive/{ufcId}")]
    Task<ApiResult> ArchiveFlashCard([Query] int ufcId, [Header("Authorization")] string authorization);
}