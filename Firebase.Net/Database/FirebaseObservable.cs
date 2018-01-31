﻿using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Firebase.Net.Http;
using Firebase.Net.Utils;
using System.Collections.Concurrent;

namespace Firebase.Net.Database
{
    public delegate void DatabaseEventHandler<T>(T obj);

    internal enum ServerEventType
    {
        Put, Patch, KeepAlive, Cancel, AuthRevoked
    }

    public class FirebaseObservable<T>
    {
        private static ConcurrentDictionary<(Type, string), FirebaseObservable<T>> Subscriptions { get; set; }

        // TODO : Handle ChildMoved event
        // public event DatabaseEventHandler<T> ChildMoved; 
        public event DatabaseEventHandler<T> ValueChanged;
        public event DatabaseEventHandler<T> ChildAdded;
        public event DatabaseEventHandler<T> ChildChanged;
        public event DatabaseEventHandler<T> ChildRemoved;

        JContainer Cache { get; set; }

        private Func<string> IdTokenFactory { get; set; }
        private UrlBuilder UrlBuilder { get; set; }
        private IFirebaseHttpClientFacade Client { get; set; }

        internal static FirebaseObservable<T> Create(UrlBuilder urlBuilder, Func<string> idTokenFactory)
        {
            FirebaseObservable<T> subscription;
            var key = (typeof(T), urlBuilder.Url);
            if (Subscriptions.TryGetValue(key, out subscription))
            {
                return subscription;
            }

            subscription = new FirebaseObservable<T>();
            subscription.UrlBuilder = urlBuilder;
            subscription.IdTokenFactory = idTokenFactory;
            subscription.Client = FirebaseHttpClientFactory.CreateFirebaseDatabaseHttpClient();
            subscription.Client.Timeout = Constants.Timeout;
            subscription.Cache = new JObject();

            Subscriptions.TryAdd(key, subscription);

            Task.Run(subscription.ListenToServerEvents);
            return subscription;
        }

        static FirebaseObservable()
        {
            Subscriptions = new ConcurrentDictionary<(Type, string), FirebaseObservable<T>>();
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
                        if (eventType == null || String.IsNullOrWhiteSpace(eventType)) continue;

                        string serializedData = await reader.ReadLineAsync();

                        ServerEvent serverEvent = ServerEvent.Parse(eventType, serializedData);
                        if (serverEvent.Type == ServerEventType.AuthRevoked) { break; }
                        if (serverEvent.Type == ServerEventType.KeepAlive) { continue; }
                        if (serverEvent.Type == ServerEventType.Cancel) { throw new PremissionDeniedException(); }

                        UpdateCache(serverEvent);
                    }
                }
            }
        }

        // Todo : Add comments to this function
        private void UpdateCache(ServerEvent serverEvent)
        {
            var token = Cache.SelectToken(serverEvent.Path, false);
            if (token != null && token.Parent == null)
            {
                Cache = serverEvent.Data as JObject ?? new JObject();
            }
            else
            {
                //if (serverEvent.Data != null)
                //{
                if (token == null)
                {
                    Cache[serverEvent.Path] = serverEvent.Data as JToken;
                    ChildAdded?.Invoke(Cache.ToObject<T>());
                }
                else
                {
                    if (token.Parent?.Parent == token.Root)
                    {
                        Cache.SelectToken(serverEvent.Path, false)?.Parent?.Remove();
                        ChildRemoved?.Invoke(Cache.ToObject<T>());
                    }
                    else
                    {
                        token.Replace(serverEvent.Data as JToken);
                        ChildChanged?.Invoke(Cache.ToObject<T>());
                    }
                }
            }

            ValueChanged?.Invoke(Cache.ToObject<T>());
        }

        class Constants
        {
            public static readonly TimeSpan Timeout = TimeSpan.FromSeconds(5200);
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