namespace Firebase.Net.Auth
{
    public class User
    {
        public string DisplayName { get; private set; }
        public string Email { get; private set; }
        public bool EmailVerified { get; private set; }
        public bool IsAnonymous { get; private set; }
        public UserMetadata Metadata { get; private set; }
        public string PhoneNumber { get; private set; }
        public string PhotoUrl { get; private set; }
        public UserInfo ProviderData { get; private set; }
        public string ProviderId { get; private set; }
        public string RefreshToken { get; private set; }
        public string UId { get; private set; }
    
        public User(string displayName, string email, bool emailVerified, bool isAnonymous,
                    UserMetadata metadata, string phoneNumber, string photoUrl, 
                    UserInfo providerData, string providerId, string refreshToken, string uid)
        {
            DisplayName = displayName;
            Email = email;
            EmailVerified = emailVerified;
            IsAnonymous = isAnonymous;
            Metadata = metadata;
            PhoneNumber = phoneNumber;
            PhotoUrl = photoUrl;
            ProviderData = providerData;
            ProviderId = providerId;
            RefreshToken = refreshToken;
            UId = uid;
        }
    }
}
