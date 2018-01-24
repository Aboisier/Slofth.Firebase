using System;
using System.Web;

namespace Firebase.Net.Extensions
{
    public static class UriBuilderExtensions
    {
        public static UriBuilder AddParam(this UriBuilder builder, string name, string value)
        {
            var query = HttpUtility.ParseQueryString(builder.Query);
            query[name] = value.ToString();
            builder.Query = query.ToString();

            return builder;
        }

        public static UriBuilder AddParam(this UriBuilder builder, string name, int value)
        {
            return AddParam(builder, name, value.ToString());
        }
    }
}
