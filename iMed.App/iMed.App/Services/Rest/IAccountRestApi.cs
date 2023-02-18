namespace iMed.App.Services.Rest;

public interface IAccountRestApi
{

    [Put("/User")]
    Task<ApiResult> EditUser([Body] UserSDto userDto, [Header("Authorization")] string authorization);
}