using Firebase.Net.Utils;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Firebase.Net.Database
{
    public abstract partial class Query
    {
        protected HttpClient Client { get; set; }
        protected UrlBuilder UrlBuilder { get; set; }
        public string Key { get; private set; }

        public Query(UrlBuilder urlBuilder, string name)
        {
            UrlBuilder = urlBuilder;
            Key = name;
            Client = new HttpClient();
        }

        /// <summary>
        /// Reads data stored at the specified reference.
        /// </summary>
        /// <typeparam name="T">Type of the data.</typeparam>
        /// <returns>The deserialized data.</returns>
        public async virtual Task<T> Once<T>()
        {
            UrlBuilder.AppendToPath(Endpoints.Json);
            var response = await Client.GetAsync(UrlBuilder.Url);

            try
            {
                return await response.Content.ReadAsAsync<T>();
            }
            catch (JsonSerializationException) { } // Means the content is null

            return default(T);
        }

        /// <summary>
        /// Saves data to the current reference. Replaces any data at that path.
        /// </summary>
        public async Task Set<T>(T value)
        {
            UrlBuilder.AppendToPath(Endpoints.Json);
            var response = await Client.PutAsJsonAsync(UrlBuilder.Url, value);
        }

        public ChildQuery Child(string name)
        {
            return new ChildQuery(UrlBuilder.AppendToPath(name), name);
        }

        public async Task<ChildQuery> Push()
        {
            var builderCopy = UrlBuilder.Copy();
            var response = await Client.PostAsJsonAsync(builderCopy.AppendToPath(Endpoints.Json).Url, new { });

            var content = await response.Content.ReadAsAsync<PostInfo>();
            return Child(content.Name);
        }

        /// <summary>
        /// Deletes the data at the specified reference.
        /// </summary>
        /// <returns></returns>
        public async Task Remove()
        {
            UrlBuilder.AppendToPath(Endpoints.Json);
            var response = await Client.DeleteAsync(UrlBuilder.Url);
        }

        class Endpoints
        {
            public static readonly string Json = ".json";
        }
    }
}
