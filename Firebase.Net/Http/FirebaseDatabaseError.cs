using Firebase.Net.Database;
using Newtonsoft.Json;
using System;

namespace Firebase.Net.Http
{
    class FirebaseDatabaseError : IFirebaseError
    {
        private static class Messages
        {
            public static readonly string CouldNotParseAuthToken = "Could not parse auth token.";
        }

        [JsonProperty("error")]
        public string Error { get; set; }

        public Exception GetCorrespondingException()
        {
            if (Error == Messages.CouldNotParseAuthToken)
                return new CouldNotParseAuthTokenException();

            return new FirebaseDatabaseException(Error);
        }
    }
}
