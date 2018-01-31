﻿using Slofth.Firebase.Net.Utils;
using System;

namespace Slofth.Firebase.Net.Database
{
    public class ChildQuery : Query
    {
        internal ChildQuery(UrlBuilder builder, string name, Func<string> idTokenFactory)
            : base(builder, name, idTokenFactory)
        { }

        /// <summary>
        /// Orders the data according to the specified property in order to filter it.
        /// </summary>
        /// <param name="property">The property used to sort. To access a nested property, use slashes (ex.: info/dimensions/height).</param>
        public FilterQuery OrderBy(string property)
        {
            return new FilterQuery(UrlBuilder.AddParam(Params.OrderBy, Quote(property)), Key, IdTokenFactory);
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