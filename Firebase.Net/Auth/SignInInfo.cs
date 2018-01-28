using Newtonsoft.Json;

namespace Firebase.Net.Auth
{
    class SignInInfo
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }
        [JsonProperty("idToken")]
        public string IdToken { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
        [JsonProperty("expiresIn")]
        public string ExpiresIn { get; set; }
        [JsonProperty("localId")]
        public string LocalId { get; set; }
        [JsonProperty("registered")]
        public string Registered { get; set; }
    }
}
