using Newtonsoft.Json;
using System.Collections.Generic;

namespace Firebase.Net.Auth
{
    class FetchProvidersResponse
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }
        [JsonProperty("allProviders")]
        public List<string> AllProviders { get; set; }
        [JsonProperty("registered")]
        public bool Registered { get; set; }
    }
}
