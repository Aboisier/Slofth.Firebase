using Slofth.Firebase.Net.Http;
using Slofth.Firebase.Net.Utils;
using System;

namespace Slofth.Firebase.Net.Database
{
    public enum DatabaseEvent
    {
        Value, ChildAdded, ChildChanged, ChildRemoved, ChildMoved
    }

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
                .AppendToPath(name);
             
            return new ChildQuery(builder, name, TokenIdFactory);
        }
    }
}
