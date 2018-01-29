using System;

namespace Firebase.Net.Http
{
    class FirebaseHttpClientFactory
    {
        public static IFirebaseHttpClientFacade CreateFirebaseAuthHttpClient()
        {
            return CreateErrorHandlerHttpClient<FirebaseAuthError>();
        }

        public static IFirebaseHttpClientFacade CreateFirebaseDatabaseHttpClient()
        {
            return CreateErrorHandlerHttpClient<FirebaseDatabaseError>();
        }

        public static IFirebaseHttpClientFacade CreateErrorHandlerHttpClient<TError>() where TError : IFirebaseError, new()
        {
            IFirebaseHttpClientFacade client = new HttpClientFacade();
            client = new FirebaseErrorHandlingDecorator<TError>(client);
            return client;
        }
    }
}
