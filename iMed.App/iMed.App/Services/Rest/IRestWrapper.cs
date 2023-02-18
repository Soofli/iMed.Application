namespace iMed.App.Services.Rest;
public interface IRestWrapper
{
    IAuthorizeRestApi AuthorizeRestApi { get; }
    IPageRestApi PageRestApi { get; }
    IAccountRestApi AccountRestApi { get; }
    IRateRestApi RateRestApi { get; }
    IPurchaseRestApi PurchaseRestApi { get; }
    IWalletRestApi WalletRestApi { get; }
    IFileRestApi FileRestApi { get; }
    ILeitnerRestApi LeitnerRestApi { get; }
}