namespace Firebase.Net.Auth
{
    class AccountInfo
    {
        public string localId { get; set; }
        public string email { get; set; }
        public bool emailVerified { get; set; }
        public string displayName { get; set; }
        public string photoUrl { get; set; }
        public string passwordHash { get; set; }
        public double passwordUpdatedAt { get; set; }
        public string validSince { get; set; }
        public bool disabled { get; set; }
        public string lastLoginAt { get; set; }
        public string createdAt { get; set; }
        public bool customAuth { get; set; }
    }
}
