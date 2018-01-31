using Newtonsoft.Json;

namespace Firebase.Net.Database
{
    public abstract partial class Query
    {
        class PostInfo
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }
}
