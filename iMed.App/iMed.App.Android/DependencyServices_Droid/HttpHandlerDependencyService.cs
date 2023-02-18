using System.Net.Http;
using iMed.App.DependencyServices;
using iMed.App.Droid.DependencyServices_Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(HttpHandlerDependencyService))]
namespace iMed.App.Droid.DependencyServices_Droid
{
    public class HttpHandlerDependencyService : IHttpHandlerDependencyService
    {
        public HttpClientHandler GetHttpClientHandler()
        {

            return new Xamarin.Android.Net.AndroidClientHandler();
        }
    }
}