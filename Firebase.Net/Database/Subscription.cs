using Firebase.Net.Http;
using Firebase.Net.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Firebase.Net.Database
{
    public delegate void DatabaseEventHandler<T>(T obj);

    internal enum ServerEventType
    {
        Put, Patch, KeepAlive, Cancel, AuthRevoked
    }

    public class Subscription<T>
    {
        //public event DatabaseEventHandler<T> Value;
        //public event DatabaseEventHandler<T> ChildAdded;
        //public event DatabaseEventHandler<T> ChildMoved;
        //public event DatabaseEventHandler<T> ChildChanged;
        public event DatabaseEventHandler<T> ChildRemoved;

        JObject Cache { get; set; }

        private Func<string> IdTokenFactory { get; set; }
        private UrlBuilder UrlBuilder { get; set; }
        private IFirebaseHttpClientFacade Client { get; set; }

        internal Subscription(UrlBuilder urlBuilder, Func<string> idTokenFactory)
        {
            UrlBuilder = urlBuilder;
            IdTokenFactory = idTokenFactory;

            Client = FirebaseHttpClientFactory.CreateFirebaseDatabaseHttpClient();
            Client.Timeout = Constants.Timeout;

            Cache = new JObject();

            Task.Run(ListenToServerEvents);
        }

        private async Task ListenToServerEvents()
        {
            UrlBuilder.AppendToPath(Endpoints.Json);

            while (true)
            {
                var urlBuilderCopy = UrlBuilder.Copy();
                urlBuilderCopy.AddParam(Params.Auth, IdTokenFactory());

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, new Uri(urlBuilderCopy.Url));
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));

                HttpResponseMessage response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                using (Stream stream = await response.Content.ReadAsStreamAsync())
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (true)
                    {
                        // An event sent from the server is built from three lines. The first is the event name, the second one is the data, and third is empty.
                        var eventType = await reader.ReadLineAsync();
                        if (eventType == null) return;

                        string serializedData = await reader.ReadLineAsync();
                        await reader.ReadLineAsync();

                        ServerEvent serverEvent = ServerEvent.Parse(eventType, serializedData);

                        if (serverEvent.Type == ServerEventType.AuthRevoked) { break; }
                        if (serverEvent.Type == ServerEventType.KeepAlive) { continue; }
                        if (serverEvent.Type == ServerEventType.Cancel) { throw new PremissionDeniedException(); }


                        if (Cache.SelectToken(serverEvent.Path).Parent == null)
                        {
                            Cache = serverEvent.Data;
                        }
                        else
                        {
                            if (serverEvent.Data != null)
                            {
                            }
                            else
                            {
                                Cache.SelectToken(serverEvent.Path).Parent.Remove();
                                ChildRemoved.Invoke(Cache.ToObject<T>());
                            }
                        }
                    }
                }
            }
        }

        private void UpdateCache()
        {

        }

        class Constants
        {
            public static readonly TimeSpan Timeout = TimeSpan.FromSeconds(600);
        }

        class Endpoints
        {
            public static readonly string Json = ".json";
        }

        class Params
        {
            public static readonly string Auth = "auth";
        }
    }
}
