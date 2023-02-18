namespace iMed.App.Utilities;
public class UserUtilities
{
    private AccessToken<UserSDto> _accessToken;

    public AccessToken<UserSDto> AccessToken
    {
        get => _accessToken;
        set
        {
            _accessToken = value;
            CrossMediaManager.Current.RequestHeaders.Remove("Authorization");
            CrossMediaManager.Current.RequestHeaders.Add("Authorization", value.BearerToken);
        }
    }

    public UserSDto User => AccessToken.User;
}
