using Firebase.Net.Utils;
using System;
using System.Net.Http;

namespace Firebase.Net.Database
{
    public class Database
    {
        private Func<string> TokenIdFactory { get; set; }

        private HttpClient Client { get; set; }
        private string DatabaseUrl { get; set; }

        public Database(string databaseUrl, Func<string> tokenIdFactory)
        {
            DatabaseUrl = databaseUrl;
            TokenIdFactory = tokenIdFactory;

            Client = new HttpClient();
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
