using System.Net.Http;
using System.Threading.Tasks;

namespace Firebase.Net.Http
{
    public abstract class HttpClientDecorator : IFirebaseHttpClientFacade
    {
        protected IFirebaseHttpClientFacade BaseComponent { get; set; }

        protected HttpClientDecorator(IFirebaseHttpClientFacade baseComponent)
        {
            BaseComponent = baseComponent;
        }

        public abstract Task<HttpResponseMessage> GetAsync(string url);
        public abstract Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T value);
        public abstract Task<HttpResponseMessage> PutAsJsonAsync<T>(string url, T value);
        public abstract Task<HttpResponseMessage> DeleteAsync(string url);
    }
}
