using iMed.App.DependencyServices;

namespace iMed.App.Services.Rest;
public class RestWrapper : IRestWrapper
{
    private static RefitSettings setting = new RefitSettings(new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
    {
        Formatting = Newtonsoft.Json.Formatting.Indented,
        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
    }));
    private readonly HttpClientHandler _clientHandler;
    public RestWrapper()
    {
        _clientHandler = Xamarin.Forms.DependencyService.Get<IHttpHandlerDependencyService>().GetHttpClientHandler();
    }
    public IAuthorizeRestApi AuthorizeRestApi { get => RestService.For<IAuthorizeRestApi>(new HttpClient(_clientHandler) { BaseAddress = new Uri(Addresses.AuthorizeController) }); } 
    public IPageRestApi PageRestApi { get => RestService.For<IPageRestApi>(new HttpClient(_clientHandler) { BaseAddress = new Uri(Addresses.PageController) }); }
    public IAccountRestApi AccountRestApi { get => RestService.For<IAccountRestApi>(new HttpClient(_clientHandler) { BaseAddress = new Uri(Addresses.AccountController) }); }
    public IRateRestApi RateRestApi { get => RestService.For<IRateRestApi>(new HttpClient(_clientHandler) { BaseAddress = new Uri(Addresses.RateController) }, setting); }
    public IPurchaseRestApi PurchaseRestApi { get => RestService.For<IPurchaseRestApi>(new HttpClient(_clientHandler) { BaseAddress = new Uri(Addresses.PurchaseController) }); }
    public IWalletRestApi WalletRestApi { get => RestService.For<IWalletRestApi>(new HttpClient(_clientHandler) { BaseAddress = new Uri(Addresses.WalletController) }); }
    public IFileRestApi FileRestApi { get => RestService.For<IFileRestApi>(new HttpClient(_clientHandler) { BaseAddress = new Uri(Addresses.FileController) }); }
    public ILeitnerRestApi LeitnerRestApi { get => RestService.For<ILeitnerRestApi>(new HttpClient(_clientHandler) { BaseAddress = new Uri(Addresses.LeitnerBoxController) }); }
}