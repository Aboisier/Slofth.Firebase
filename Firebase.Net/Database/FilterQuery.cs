using Firebase.Net.Utils;
using System.Net.Http;

namespace Firebase.Net.Database
{
    public class FilterQuery : Query
    {
        public FilterQuery(UrlBuilder urlBuilder, string name)
             : base(urlBuilder, name)
        { }
    }
}
