using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Core.Collections
{
    public static class ListExtensions
    {
        public static IList<T> AddRange<T>(this IList<T> collection, IEnumerable<T> range)
        {
            foreach (T item in range)
                collection.Add(item);

            return collection;
        }
    }
}
