namespace Firebase.Net.Auth
{
    public class UserMetadata
    {
        public string CreationTime { get; private set; }
        public string LastSignInTime { get; private set; }

        public UserMetadata(string creationTime, string lastSignInTime)
        {
            CreationTime = creationTime;
            LastSignInTime = lastSignInTime;
        }
    }
}
