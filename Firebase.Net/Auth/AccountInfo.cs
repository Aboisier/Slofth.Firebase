using Newtonsoft.Json;

namespace Firebase.Net.Auth
{
    class AccountInfo
    {
        [JsonProperty("localId")]
        public string LocalId { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("emailVerified")]
        public bool EmailVerified { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("photoUrl")]
        public string PhotoUrl { get; set; }
        [JsonProperty("passwordHash")]
        public string PasswordHash { get; set; }
        [JsonProperty("passwordUpdatedAt")]
        public double PasswordUpdatedAt { get; set; }
        [JsonProperty("validSince")]
        public string ValidSince { get; set; }
        [JsonProperty("disabled")]
        public bool Disabled { get; set; }
        [JsonProperty("lastLoginAt")]
        public string LastLoginAt { get; set; }
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }
        [JsonProperty("customAuth")]
        public bool CustomAuth { get; set; }
    }
}
