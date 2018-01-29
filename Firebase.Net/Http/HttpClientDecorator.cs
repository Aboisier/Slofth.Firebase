using System.Net.Http;
using System.Threading.Tasks;

namespace Firebase.Net.Http
{
    public abstract class HttpClientDecorator : IHttpClientFacade
    {
        protected IHttpClientFacade BaseComponent { get; set; }

        protected HttpClientDecorator(IHttpClientFacade baseComponent)
        {
            BaseComponent = baseComponent;
        }

        public abstract Task<HttpResponseMessage> GetAsync(string url);
        public abstract Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T value);
        public abstract Task<HttpResponseMessage> PutAsJsonAsync<T>(string url, T value);
        public abstract Task<HttpResponseMessage> DeleteAsync(string url);
    }
}
