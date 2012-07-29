using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Core.Collections
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Checks to see if IEnumerable contains specified item
        /// </summary>
        /// <typeparam name="T">type of item in collection</typeparam>
        /// <param name="collection">The collection to check</param>
        /// <param name="predicate">where predicate</param>
        /// <returns>true if predicate yields 1 or more results, otherwise false</returns>
        public static bool Contains<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            return collection.FirstOrDefault(predicate) != null;
        }

        /// <summary>
        /// Converts a list of string values seperated by a delimeter
        /// </summary>
        /// <param name="collection">collection to convert</param>
        /// <param name="seperator">seperator to use</param>
        /// <returns>string</returns>
        public static string ToSeperatedString<T>(this IEnumerable<T> collection, Func<T, string> property, string seperator)
        {
            var propertyValueCollection = collection.Select(property);
            return propertyValueCollection.ToSeperatedString(seperator);
        }

        /// <summary>
        /// Converts a list of string values seperated by a delimeter
        /// </summary>
        /// <param name="collection">collection to convert</param>
        /// <param name="seperator">seperator to use</param>
        /// <returns>string</returns>
        public static string ToSeperatedString(this IEnumerable<string> collection, string seperator)
        {
            var builder = new StringBuilder();

            foreach (string item in collection)
                builder.AppendFormat("{0}{1}", item, seperator);

            return builder.ToString();
        }
    }
}
