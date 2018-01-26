using Firebase.Net.Utils;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Firebase.Net.Database
{
    public class ChildQuery : Query
    {
        public ChildQuery(UrlBuilder builder, string name)
            : base(builder, name)
        { }

        public FilterQuery OrderBy(string property)
        {
            return new FilterQuery(UrlBuilder.AddParam("orderBy", property), Key);

        }
    }
}
