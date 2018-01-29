using Firebase.Net.Http;
using Firebase.Net.Utils;
using System;

namespace Firebase.Net.Database
{
    public class Database
    {
        private Func<string> TokenIdFactory { get; set; }

        private IFirebaseHttpClientFacade Client { get; set; }
        private string DatabaseUrl { get; set; }

        public Database(string databaseUrl, Func<string> tokenIdFactory)
        {
            DatabaseUrl = databaseUrl;
            TokenIdFactory = tokenIdFactory;

            Client = FirebaseHttpClientFactory.CreateFirebaseDatabaseHttpClient();
        }

        public ChildQuery Ref(string name)
        {
            var builder = UrlBuilder
                .Create(DatabaseUrl)
                .AppendToPath(name)
                .AddParam(Params.Auth, TokenIdFactory());

            return new ChildQuery(builder, name);
        }

        class Params
        {
            public static readonly string Auth = "auth";
        }
    }
}
