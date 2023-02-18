namespace iMed.App.Models;

public static class Addresses
{
#if DEBUG
    //public static string BaseAddress { get; set; } = "http://192.168.1.85:49153";
    public static string BaseAddress { get; set; } = "https://api.imedapp.ir";
    public static string BaseStorageAddress { get; set; } = "https://storage.imedapp.ir";
    //public static string BaseStorageAddress { get; set; } = "http://192.168.0.102:49153/api/v1/File/video";
#else
    public static string BaseAddress { get; set; } = "https://api.imedapp.ir";
    public static string BaseStorageAddress { get; set; } = "https://storage.imedapp.ir";
#endif
    public static string BaseApiAddress { get; } = $"{BaseAddress}/api/v1";
    public static string AuthorizeController { get; } = $"{BaseApiAddress}/authorize";
    public static string PageController { get; } = $"{BaseApiAddress}/page";
    public static string AccountController { get; } = $"{BaseApiAddress}/account";
    public static string RateController { get; } = $"{BaseApiAddress}/rate";
    public static string PurchaseController { get; } = $"{BaseApiAddress}/purchase";
    public static string WalletController { get; } = $"{BaseApiAddress}/wallet";
    public static string FileController { get; } = $"{BaseApiAddress}/file";
    public static string LeitnerBoxController { get; } = $"{BaseApiAddress}/LeitnerBox";
    public static string DownloadIdentityImage { get; } = $"{BaseApiAddress}/file/identityimage";
    public static string DownloadImage { get; } = $"{BaseStorageAddress}";
    public static string DownloadVideo { get; } = $"{BaseStorageAddress}/Videos";
    public static string DownloadHandout { get; } = $"{BaseApiAddress}/file/handout";
}