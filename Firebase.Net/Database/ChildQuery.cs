using Firebase.Net.Utils;

namespace Firebase.Net.Database
{
    public class ChildQuery : Query
    {
        public ChildQuery(UrlBuilder builder, string name)
            : base(builder, name)
        { }

        /// <summary>
        /// Orders the data according to the specified property in order to filter it.
        /// </summary>
        /// <param name="property">The property used to sort. To access a nested property, use slashes (ex.: info/dimensions/height).</param>
        public FilterQuery OrderBy(string property)
        {
            return new FilterQuery(UrlBuilder.AddParam(Params.OrderBy, Quote(property)), Key);
        }

        public FilterQuery OrderByKey()
        {
            return OrderBy("$key");
        }

        public FilterQuery OrderByPriority()
        {
            return OrderBy("$priority");
        }

        public FilterQuery OrderByValue()
        {
            return OrderBy("$value");
        }

        class Params
        {
            public static readonly string OrderBy = "orderBy";
        }
    }
}
