using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Firebase.Net.Auth
{
    public class Auth
    {
        private static class Endpoints
        {
            public static readonly string ApiBase = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/";
            public static readonly string AccountInfo = "getAccountInfo";
            public static readonly string SignUp = "signupNewUser";
            public static readonly string SignIn = "verifyPassword";
            public static readonly string SendConfirmationEmail = "getOobConfirmationCode";
            public static readonly string PasswordReset = "resetPassword";
        }

        private static class RequestTypes
        {
            public static readonly string PasswordReset = "PASSWORD_RESET";
            public static readonly string VerifyEmail= "VERIFY_EMAIL";
        }

        private string ApiKey { get; set; }
        private HttpClient Client { get; set; }
        private string EndpointKeySuffix => $"?key={ApiKey}";

        public Auth(string apiKey)
        {
            ApiKey = apiKey;
            Client = new HttpClient();
            Client.BaseAddress = new Uri(Endpoints.ApiBase);
        }

        private async Task<AccountInfo> GetAccountInfo(string idToken)
        {
            string url = Endpoints.AccountInfo + EndpointKeySuffix;
            var body = new { idToken };
            var response = await Client.PostAsJsonAsync(url, body);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsAsync<Error>();
                if (error != null)
                    throw error.GetCorrespondingException();

                throw new FirebaseAuthException();
            }
            return await response.Content.ReadAsAsync<AccountInfo>();
        }

        public async Task<User> CreateUserWithEmailAndPassword(string email, string password)
        {
            string url = Endpoints.SignUp + EndpointKeySuffix;
            var body = new { email, password, returnSecureToken = true };
            var response = await Client.PostAsJsonAsync(url, body);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsAsync<Error>();
                if (error != null)
                    throw error.GetCorrespondingException();

                throw new FirebaseAuthException();
            }

            var signUpInfo = await response.Content.ReadAsAsync<SignInInfo>();
            var accountInfo = await GetAccountInfo(signUpInfo.idToken);
            var metadata = new UserMetadata(accountInfo.createdAt, accountInfo.lastLoginAt);
            var user = new User(accountInfo.displayName, accountInfo.email, accountInfo.emailVerified,
                                false, metadata, null, accountInfo.photoUrl, null, null,
                                signUpInfo.refreshToken, signUpInfo.localId);
            return user;
        } 

        public async Task<User> SignInWithEmailAndPassword(string email, string password)
        {
            string url = Endpoints.SignIn + EndpointKeySuffix;
            var body = new { email, password, returnSecureToken = true };
            var response = await Client.PostAsJsonAsync(url, body);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsAsync<Error>();
                if (error != null)
                    throw error.GetCorrespondingException();

                throw new FirebaseAuthException();
            }

            var signInInfo = await response.Content.ReadAsAsync<SignInInfo>();
            var accountInfo = await GetAccountInfo(signInInfo.idToken);
            var metadata = new UserMetadata(accountInfo.createdAt, accountInfo.lastLoginAt);
            var user = new User(accountInfo.displayName, accountInfo.email, accountInfo.emailVerified,
                                false, metadata, null, accountInfo.photoUrl, null, null,
                                signInInfo.refreshToken, signInInfo.localId);
            return user;
        }

        public async Task<User> SignInAnonymously()
        {
            string url = Endpoints.SignUp + EndpointKeySuffix;
            var body = new { returnSecureToken = true };
            var response = await Client.PostAsJsonAsync(url, body);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsAsync<Error>();
                if (error != null)
                    throw error.GetCorrespondingException();

                throw new FirebaseAuthException();
            }

            var signInInfo = await response.Content.ReadAsAsync<SignInInfo>();
            var user = new User(null, null, false, true, null, null, null, null, null,
                                signInInfo.refreshToken, signInInfo.localId);
            return user;
        }

        public async Task<bool> SendPasswordResetEmail(User user)
        {
            string url = Endpoints.SendConfirmationEmail + EndpointKeySuffix;
            var body = new { requestType = RequestTypes.PasswordReset, email = user.Email };
            var response = await Client.PostAsJsonAsync(url, body);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsAsync<Error>();
                if (error != null)
                    throw error.GetCorrespondingException();

                throw new FirebaseAuthException();
            }
            return true;
        }

        public async Task<bool> VerifyPasswordResetCorde(string code)
        {
            string url = Endpoints.PasswordReset + EndpointKeySuffix;
            var body = new { oobCode = code };
            var response = await Client.PostAsJsonAsync(url, body);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsAsync<Error>();
                if (error != null)
                    throw error.GetCorrespondingException();

                throw new FirebaseAuthException();
            }
            return true;
        }

        public async Task<bool> ConfirmPasswordReset(string code, string newPassword)
        {
            string url = Endpoints.PasswordReset + EndpointKeySuffix;
            var body = new { oobCode = code, newPassword };
            var response = await Client.PostAsJsonAsync(url, body);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsAsync<Error>();
                if (error != null)
                    throw error.GetCorrespondingException();

                throw new FirebaseAuthException();
            }
            return true;
        }


    }
}
