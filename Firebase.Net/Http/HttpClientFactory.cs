using System;

namespace Firebase.Net.Http
{
    class HttpClientFactory
    {
        public static IHttpClientFacade Create()
        {
            return Create(null);
        }

        public static IHttpClientFacade Create(Uri baseUri)
        {
            IHttpClientFacade client = new HttpClientFacade(baseUri);
            client = new FirebaseErrorHandlingDecorator(client);
            return client;
        }
    }
}
