using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Slofth.Firebase.Http
{
    class HttpClientFacade : IFirebaseHttpClientFacade
    {
        private HttpClient Client { get; set; }

        public HttpRequestHeaders Headers => Client.DefaultRequestHeaders;

        public TimeSpan Timeout
        {
            get => Client.Timeout;
            set => Client.Timeout = value;
        }
        public HttpClientFacade()
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.AllowAutoRedirect = true;
            Client = new HttpClient(httpClientHandler, true);
        }

        public HttpClientFacade(Uri baseAddress)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.AllowAutoRedirect = true;
            Client = new HttpClient(httpClientHandler, true);
            Client.BaseAddress = baseAddress;
        }

        public HttpClientFacade(HttpClient client)
        {
            Client = client;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)
        {
            return await Client.SendAsync(request, completionOption);
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T value)
        {
            return await Client.PostAsJsonAsync(url, value);
        }

        public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string url, T value)
        {
            return await Client.PutAsJsonAsync(url, value);
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await Client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await Client.DeleteAsync(url);
        }
    }
}
