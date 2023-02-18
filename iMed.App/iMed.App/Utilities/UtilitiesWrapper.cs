namespace iMed.App.Utilities;
public class UtilitiesWrapper
{
    private static UtilitiesWrapper _instance;
    public static UtilitiesWrapper Instance
    {
        get
        {
            if (_instance == null)
                _instance = new UtilitiesWrapper();
            return _instance;
        }
    }

    public UserUtilities UserUtilities { get; } = new UserUtilities();
    public string BearerToken
    {
        get => UserUtilities.AccessToken.BearerToken;
    }
    public PopUpUtilities PopUpUtilities { get; } = new PopUpUtilities();
}
