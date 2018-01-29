using Firebase.Net.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Firebase.Net.Auth
{
    public class Auth
    {
        private static class Endpoints
        {
            public static readonly string ApiBase = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/";
            public static readonly string GetAccountInfo = "getAccountInfo";
            public static readonly string SetAccountInfo = "setAccountInfo";
            public static readonly string SignUp = "signupNewUser";
            public static readonly string SignIn = "verifyPassword";
            public static readonly string SendConfirmationEmail = "getOobConfirmationCode";
            public static readonly string ResetPassword = "resetPassword";
            public static readonly string DeleteAccount = "deleteAccount";
            public static readonly string VerifyAssertion = "verifyAssertion";
            public static readonly string CreateAuthUri = "ceateAuthUri";
            public static readonly string VerifyCustomToken = "verifyCustomToken";
        }

        private static class RequestTypes
        {
            public static readonly string PasswordReset = "PASSWORD_RESET";
            public static readonly string VerifyEmail = "VERIFY_EMAIL";
        }

        public event EventHandler<User> OnAuthStateChanged;

        private string ApiKey { get; set; }
        private IHttpClientFacade Client { get; set; }
        private string EndpointKeySuffix => $"?key={ApiKey}";

        private User currentUser;
        public User CurrentUser
        {
            get => currentUser;
            private set
            {
                if (value != currentUser)
                {
                    currentUser = value;
                    OnAuthStateChanged.Invoke(this, currentUser);
                }
            }
        }

        public Auth(string apiKey)
        {
            ApiKey = apiKey;
            Client = Http.HttpClientFactory.Create(new Uri(Endpoints.ApiBase));
        }

        public async Task<User> CreateUserWithEmailAndPassword(string email, string password)
        {
            string url = Endpoints.SignUp + EndpointKeySuffix;
            var body = new { email, password, returnSecureToken = true };
            var response = await Client.PostAsJsonAsync(url, body);

            var signUpInfo = await response.Content.ReadAsAsync<SignInInfo>();
            var accountInfo = await GetAccountInfo(signUpInfo.IdToken);
            var metadata = new UserMetadata(accountInfo.CreatedAt, accountInfo.LastLoginAt);
            var user = new User(accountInfo.DisplayName, accountInfo.Email, accountInfo.EmailVerified,
                                false, metadata, null, accountInfo.PhotoUrl, null, null,
                                signUpInfo.RefreshToken, signUpInfo.LocalId);
            CurrentUser = user;
            return user;
        }

        public async Task<User> SignInWithEmailAndPassword(string email, string password)
        {
            string url = Endpoints.SignIn + EndpointKeySuffix;
            var body = new { email, password, returnSecureToken = true };
            var response = await Client.PostAsJsonAsync(url, body);

            var signInInfo = await response.Content.ReadAsAsync<SignInInfo>();
            var accountInfo = await GetAccountInfo(signInInfo.IdToken);
            var metadata = new UserMetadata(accountInfo.CreatedAt, accountInfo.LastLoginAt);
            var user = new User(accountInfo.DisplayName, accountInfo.Email, accountInfo.EmailVerified,
                                false, metadata, null, accountInfo.PhotoUrl, null, null,
                                signInInfo.RefreshToken, signInInfo.LocalId);
            CurrentUser = user;
            return user;
        }

        public async Task<User> SignInAnonymously()
        {
            string url = Endpoints.SignUp + EndpointKeySuffix;
            var body = new { returnSecureToken = true };
            var response = await Client.PostAsJsonAsync(url, body);

            var signInInfo = await response.Content.ReadAsAsync<SignInInfo>();
            var user = new User(null, null, false, true, null, null, null, null, null,
                                signInInfo.RefreshToken, signInInfo.LocalId);
            CurrentUser = user;
            return user;
        }

        public async Task SignOut()
        {
            CurrentUser = null;
        }

        public async Task<List<string>> FetchProvidersForEmail(string email)
        {
            string url = Endpoints.CreateAuthUri + EndpointKeySuffix;
            var body = new { identifier = email, continueUri = "" };
            var response = await Client.PostAsJsonAsync(url, body);
            var providersResponse = await response.Content.ReadAsAsync<FetchProvidersResponse>();
            return providersResponse.AllProviders;
        }

        public async Task SendPasswordResetEmail(string email)
        {
            string url = Endpoints.SendConfirmationEmail + EndpointKeySuffix;
            var body = new { requestType = RequestTypes.PasswordReset, email };
            var response = await Client.PostAsJsonAsync(url, body);
        }

        public async Task<bool> VerifyPasswordResetCode(string code)
        {
            string url = Endpoints.ResetPassword + EndpointKeySuffix;
            var body = new { oobCode = code };
            var response = await Client.PostAsJsonAsync(url, body);
            return true;
        }

        public async Task ConfirmPasswordReset(string code, string newPassword)
        {
            string url = Endpoints.ResetPassword + EndpointKeySuffix;
            var body = new { oobCode = code, newPassword };
            var response = await Client.PostAsJsonAsync(url, body);
        }

        public async Task ChangeEmail(string idToken, string newEmail)
        {
            string url = Endpoints.SetAccountInfo + EndpointKeySuffix;
            var body = new { idToken, email = newEmail, returnSecureToken = true };
            var response = await Client.PostAsJsonAsync(url, body);
        }

        public async Task ChangePassword(string idToken, string newPassword)
        {
            string url = Endpoints.SetAccountInfo + EndpointKeySuffix;
            var body = new { idToken, password = newPassword, returnSecureToken = true };
            var response = await Client.PostAsJsonAsync(url, body);
        }

        private async Task<AccountInfo> GetAccountInfo(string idToken)
        {
            string url = Endpoints.GetAccountInfo + EndpointKeySuffix;
            var body = new { idToken };
            var response = await Client.PostAsJsonAsync(url, body);

            return await response.Content.ReadAsAsync<AccountInfo>();
        }

        public async Task LinkWithEmailAndPassword(string idToken, string email, string password)
        {
            string url = Endpoints.SetAccountInfo + EndpointKeySuffix;
            var body = new { idToken, email, password, returnSecureToken = true };
            var response = await Client.PostAsJsonAsync(url, body);
        }

        public async Task SendEmailVerificationEmail(string idToken)
        {
            string url = Endpoints.SendConfirmationEmail + EndpointKeySuffix;
            var body = new { requestType = RequestTypes.VerifyEmail, idToken };
            var response = await Client.PostAsJsonAsync(url, body);
        }

        public async Task ConfirmEmailVerification(string code)
        {
            string url = Endpoints.SetAccountInfo + EndpointKeySuffix;
            var body = new { oobCode = code };
            var response = await Client.PostAsJsonAsync(url, body);
        }

        public async Task DeleteAccount(string idToken)
        {
            string url = Endpoints.DeleteAccount + EndpointKeySuffix;
            var body = new { idToken };
            var response = await Client.PostAsJsonAsync(url, body);
        }
    }
}
