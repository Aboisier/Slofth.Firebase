using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Firebase.Net.Auth
{
    public class Auth
    {
        private static class Constants
        {
            public static string BaseEndpoint = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/";

            public static string AccountInfoEndpoint = "getAccountInfo";
            public static string SignUpEndpoint = "signupNewUser";
            public static string SignInEndpoint = "verifyPassword";
            public static string SendConfirmationEmailEndpoint = "getOobConfirmationCode";
            public static string PasswordResetEndpoint = "resetPassword";

            public static string PasswordResetRequest = "PASSWORD_RESET";
            public static string VerifyEmailRequestes = "VERIFY_EMAIL";
        }

        private string ApiKey { get; set; }
        private HttpClient Client { get; set; }
        private string EndpointKeySuffix => $"?key={ApiKey}";

        public Auth(string apiKey)
        {
            ApiKey = apiKey;
            Client = new HttpClient();
            Client.BaseAddress = new Uri(Constants.BaseEndpoint);
        }

        private async Task<AccountInfo> GetAccountInfo(string idToken)
        {
            string url = Constants.AccountInfoEndpoint + EndpointKeySuffix;
            var body = new { idToken };
            var response = await Client.PostAsJsonAsync(url, body);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<AccountInfo>();
            }
            return null;
        }

        public async Task<User> CreateUserWithEmailAndPassword(string email, string password)
        {
            string url = Constants.SignUpEndpoint + EndpointKeySuffix;
            var body = new { email, password, returnSecureToken = true };
            var response = await Client.PostAsJsonAsync(url, body);
            if (response.IsSuccessStatusCode)
            {
                var signUpInfo = await response.Content.ReadAsAsync<SignInInfo>();
                var accountInfo = await GetAccountInfo(signUpInfo.idToken);
                var metadata = new UserMetadata(accountInfo.createdAt, accountInfo.lastLoginAt);
                var user = new User(accountInfo.displayName, accountInfo.email, accountInfo.emailVerified,
                                    false, metadata, null, accountInfo.photoUrl, null, null, 
                                    signUpInfo.refreshToken, signUpInfo.localId);
                return user;
            }
            return null;
        } 

        public async Task<User> SignInWithEmailAndPassword(string email, string password)
        {
            string url = Constants.SignInEndpoint + EndpointKeySuffix;
            var body = new { email, password, returnSecureToken = true };
            var response = await Client.PostAsJsonAsync(url, body);
            if (response.IsSuccessStatusCode)
            {
                var signInInfo = await response.Content.ReadAsAsync<SignInInfo>();
                var accountInfo = await GetAccountInfo(signInInfo.idToken);
                var metadata = new UserMetadata(accountInfo.createdAt, accountInfo.lastLoginAt);
                var user = new User(accountInfo.displayName, accountInfo.email, accountInfo.emailVerified,
                                    false, metadata, null, accountInfo.photoUrl, null, null,
                                    signInInfo.refreshToken, signInInfo.localId);
                return user;
            }
            return null;
        }

        public async Task<User> SignInAnonymously()
        {
            string url = Constants.SignUpEndpoint + EndpointKeySuffix;
            var body = new { returnSecureToken = true };
            var response = await Client.PostAsJsonAsync(url, body);
            if (response.IsSuccessStatusCode)
            {
                var signInInfo = await response.Content.ReadAsAsync<SignInInfo>();
                var user = new User(null, null, false, true, null, null, null, null, null,
                                    signInInfo.refreshToken, signInInfo.localId);
                return user;
            }
            return null;
        }

        public async Task<bool> SendPasswordResetEmail(User user)
        {
            string url = Constants.SendConfirmationEmailEndpoint + EndpointKeySuffix;
            var body = new { requestType = Constants.PasswordResetRequest, email = user.Email };
            var response = await Client.PostAsJsonAsync(url, body);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> VerifyPasswordResetCorde(string code)
        {
            string url = Constants.PasswordResetEndpoint + EndpointKeySuffix;
            var body = new { oobCode = code };
            var response = await Client.PostAsJsonAsync(url, body);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ConfirmPasswordReset(string code, string newPassword)
        {
            string url = Constants.PasswordResetEndpoint + EndpointKeySuffix;
            var body = new { oobCode = code, newPassword };
            var response = await Client.PostAsJsonAsync(url, body);
            return response.IsSuccessStatusCode;
        }
    }
}
