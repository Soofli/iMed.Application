namespace iMed.App.Services.Rest;
public interface ICrudApiRest<T, in TKey> where T : class
{
    [Post("")]
    Task<ApiResult<T>> Create([Body] T payload, [Header("Authorization")] string authorization);

    [Get("")]
    Task<ApiResult<List<T>>> ReadAll([Header("Authorization")] string authorization);

    [Get("/{key}")]
    Task<ApiResult<T>> ReadOne(TKey key, [Header("Authorization")] string authorization);

    [Put("")]
    Task<ApiResult> Update([Body] T payload, [Header("Authorization")] string authorization);

    [Delete("/{key}")]
    Task<ApiResult> Delete(TKey key, [Header("Authorization")] string authorization);

}
public interface ICrudDtoApiRest<T, TDto, in TKey> where T : class where TDto : class
{
    [Post("")]
    Task<ApiResult<T>> Create([Body] T payload, [Header("Authorization")] string authorization);
    [Post("")]
    Task<ApiResult<T>> Create([Body] TDto payload, [Header("Authorization")] string authorization);

    [Get("")]
    Task<ApiResult<List<TDto>>> ReadAll([Header("Authorization")] string authorization);

    [Get("/{key}")]
    Task<ApiResult<TDto>> ReadOne(TKey key, [Header("Authorization")] string authorization);

    [Put("")]
    Task<ApiResult> Update([Body] T payload, [Header("Authorization")] string authorization);
    [Put("")]
    Task<ApiResult> Update([Body] TDto payload, [Header("Authorization")] string authorization);

    [Delete("/{key}")]
    Task<ApiResult> Delete(TKey key, [Header("Authorization")] string authorization);

}