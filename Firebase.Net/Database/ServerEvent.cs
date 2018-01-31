using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Firebase.Net.Database
{
    class ServerEvent
    {
        public ServerEventType Type { get; private set; }
        public string Path { get; private set; }
        public object Data { get; private set; }

        private ServerEvent(ServerEventType eventType, string path, object data)
        {
            Type = eventType;
            Path = path;
            Data = data;
        }

        internal static ServerEvent Parse(string serializedEvent, string serializedData)
        {
            if (serializedEvent == null || serializedData == null) return null;

            var type = ToDatabaseEvent(serializedEvent);

            dynamic content = JsonConvert.DeserializeObject(serializedData.Split(new char[] { ':' }, 2)[1]);
            if (content == null)
            {
                return new ServerEvent(type, null, null);
            }
            else
            {
                // The data part looks like this: data: { path: "path/to/resource", data: {data: 2, otherData: "helo" }}

                // We need to separate the keys with dots instead of slashes.
                var path = (string)content["path"];
                path = path?.Replace('/', '.')?.TrimStart('.');

                // The data is a json object. It is null if we deal with a remove operation.
                dynamic data = content["data"];

                return new ServerEvent(type, path, data);
            }
        }

        private static ServerEventType ToDatabaseEvent(string eventName)
        {
            var sauce = eventName.Split(':')[1].Trim();

            switch (sauce)
            {
                case "put":
                    return ServerEventType.Put;

                case "patch":
                    return ServerEventType.Patch;

                case "keep-alive":
                    return ServerEventType.KeepAlive;

                case "auth_revoked":
                    return ServerEventType.AuthRevoked;

                case "cancel":
                    return ServerEventType.Cancel;

                default:
                    throw new UnknownServerEventException();
            }
        }
    }
}
