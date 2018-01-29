using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Firebase.Net.Http
{
    class HttpClientFacade : IFirebaseHttpClientFacade
    {
        private HttpClient Client { get; set; }

        public HttpClientFacade()
        {
            Client = new HttpClient();
        }

        public HttpClientFacade(Uri baseAddress)
        {
            Client = new HttpClient();
            Client.BaseAddress = baseAddress;
        }

        public HttpClientFacade(HttpClient client)
        {
            Client = client;
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
