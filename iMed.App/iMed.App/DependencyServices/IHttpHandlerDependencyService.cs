namespace iMed.App.DependencyServices;

public interface IHttpHandlerDependencyService
{
    HttpClientHandler GetHttpClientHandler();
}