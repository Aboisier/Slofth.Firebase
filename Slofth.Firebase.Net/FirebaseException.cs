using System;

namespace Slofth.Firebase.Net
{
    public class FirebaseException : Exception
    {
        public FirebaseException(string message = null) : base(message) { }
    }
}
