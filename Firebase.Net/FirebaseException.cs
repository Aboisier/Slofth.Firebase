using System;

namespace Firebase.Net
{
    public class FirebaseException : Exception
    {
        public FirebaseException(string message = null) : base(message) { }
    }
}
