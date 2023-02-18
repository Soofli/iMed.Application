namespace iMed.App.Services.Rest;

public interface IFileRestApi
{
    [Post("/UploadIdentityImage")]
    Task<ApiResult<ResponseFile>> UploadIdentityImage([Body] FileUploadRequest payload, [Header("Authorization")] string authorization);
}