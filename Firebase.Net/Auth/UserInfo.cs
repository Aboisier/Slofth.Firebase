namespace Firebase.Net.Auth
{
    public class UserInfo
    {
        public string DisplayName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string PhotoUrl { get; private set; }
        public string ProviderId { get; private set; }
        public string UId { get; private set; }

        public UserInfo(string displayName, string email, string phoneNumber, string photoUrl, string providerId, string uid)
        {
            DisplayName = displayName;
            Email = email;
            PhoneNumber = phoneNumber;
            PhotoUrl = photoUrl;
            ProviderId = providerId;
            UId = uid;
        }
    }
}
