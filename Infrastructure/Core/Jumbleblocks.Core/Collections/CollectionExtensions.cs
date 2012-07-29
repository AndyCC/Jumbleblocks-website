using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Core.Collections
{
    /// <summary>
    /// Extensions for collection
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds item to collection from range
        /// </summary>
        /// <typeparam name="T">type of items in collection</typeparam>
        /// <param name="collection">collection to add to</param>
        /// <param name="range">range to add</param>
        /// <returns>ICollection of T</returns>
        public static ICollection<T> AddRange<T>(this ICollection<T> collection, IEnumerable<T> range)
        {
            foreach (T item in range)
                collection.Add(item);

            return collection;
        }

        /// <summary>
        /// Adds item to collection from range if it is not already in collection
        /// </summary>
        /// <typeparam name="T">type of items in collection</typeparam>
        /// <param name="collection">collection to add to</param>
        /// <param name="range">range to add</param>
        /// <param name="comparer">optional comparer of T</param>
        /// <returns>ICollection of T</returns>
        public static ICollection<T> AddDistinctFromRange<T>(this ICollection<T> collection, IEnumerable<T> range, IEqualityComparer<T> comparer = null)
        {
            foreach (T item in range)
            {
                bool isContained = false;

                if (comparer == null)
                    isContained = collection.Contains(item);
                else
                    isContained = collection.Contains(item, comparer);

                if (!isContained)
                    collection.Add(item);
            }

            return collection;
        }
    }
}
