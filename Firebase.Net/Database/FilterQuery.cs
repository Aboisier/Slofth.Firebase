using Firebase.Net.Utils;
using System.Net.Http;
using System.Threading.Tasks;

namespace Firebase.Net.Database
{
    public class FilterQuery : Query
    {
        public FilterQuery(UrlBuilder urlBuilder, string name)
             : base(urlBuilder, name)
        { }

        public FilterQuery LimitToFirst(int value)
        {
            return new FilterQuery(UrlBuilder.AddParam(Params.LimitToFirst, value), Key);
        }

        public FilterQuery LimitToLast(int value)
        {
            return new FilterQuery(UrlBuilder.AddParam(Params.LimitToLast, value), Key);
        }

        public FilterQuery StartAt(string value)
        {
            return new FilterQuery(UrlBuilder.AddParam(Params.StartAt, value), Key);
        }

        public FilterQuery EndAt(string value)
        {
            return new FilterQuery(UrlBuilder.AddParam(Params.EndAt, value), Key);
        }

        public FilterQuery EqualTo(string value)
        {
            return new FilterQuery(UrlBuilder.AddParam(Params.EqualTo, value), Key);
        }

        //public override Task<T> Once<T>()
        //{
        //}

        class Params
        {
            public static readonly string LimitToFirst = "limitToFirst";
            public static readonly string LimitToLast = "limitToLast";
            public static readonly string StartAt = "startAt";
            public static readonly string EndAt = "endAt";
            public static readonly string EqualTo = "equalTo";
        }
    }
}
