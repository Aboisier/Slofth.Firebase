using System.Net.Http;
using System.Threading.Tasks;

namespace Firebase.Net.Utils
{
    public class HandledHttpClient
    {
        private HttpClient Client { get; set; }

        public HandledHttpClient()
        {
            Client = new HttpClient();
        }

        public HandledHttpClient(HttpClient client)
        {
            Client = client;
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T value)
        {
            var response = await Client.PostAsJsonAsync(url, value);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsAsync<Error>();

                if (error != null)
                    throw error.GetCorrespondingException();

                throw new FirebaseException();
            }

            return response;
        }
    }
}
