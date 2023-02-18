namespace iMed.App.Services.Rest;
public interface IAuthorizeRestApi
{
    [Post("/VerifyPhoneNumber")]
    Task<ApiResult<bool>> VerifyPhoneNumberAsync([Query] string phoneNumber);

    [Post("/SignUpUser")]
    Task<ApiResult<AccessToken<UserSDto>>> SignUpUserAsync([Body] SignUpRequestDto signUpRequestDto);

    [Post("/LoginUser")]
    Task<ApiResult<AccessToken<UserSDto>>> LoginUserAsync([Body] LoginRequestDto loginRequestDto);

    [Post("/ForgetPassword")]
    Task<ApiResult<bool>> ForgetPasswordAsync([Query] string phoneNumber);
}