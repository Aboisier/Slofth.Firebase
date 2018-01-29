using System;

namespace Firebase.Net.Database
{
    public class FirebaseDatabaseException : Exception
    {
        public FirebaseDatabaseException(string message = null) : base(message) { }
    }

    public class CouldNotParseAuthTokenException : Exception
    {
        public CouldNotParseAuthTokenException(string message = null) : base(message) { }
    }
}
