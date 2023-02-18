namespace iMed.App.Services.Rest;

public interface IWalletRestApi
{
    [Post("/IncreaseInventory")]
    Task<ApiResult<IncreaseInventoryResponseDto>> IncreaseInventory([Query] long amount, [Header("Authorization")] string authorization);
}