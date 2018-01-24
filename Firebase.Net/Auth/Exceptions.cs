using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firebase.Net.Auth
{
    public class InvalidRefreshTokenException : Exception { }

    public class EmailExistsException : Exception { }

    public class OperationNotAllowedException : Exception { }

    public class TooManyAttemptsException : Exception { }

    public class EmailNotFoundException : Exception { }

    public class InvalidPasswordException : Exception { }

    public class UserDisabledException : Exception { }

    public class InvalidIdpResponseException : Exception { }

    public class ExpiredOobCodeException : Exception { }

    public class InvalidOobCodeException : Exception { }

    public class InvalidIdTokenException : Exception { }

    public class WeakPasswordException : Exception { }

    public class UserNotFoundException : Exception { }

    public class CredentialTooOldException : Exception { }

    public class TokenExpiredException : Exception { }

    public class FederateUserIdAlreadyLinkedException : Exception { }

    public class FirebaseAuthException : Exception { }
}
