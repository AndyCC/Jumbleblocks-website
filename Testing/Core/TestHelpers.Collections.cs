using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Jumbleblocks.Testing
{
    public static partial class TestHelpers
    {
        public static void ShouldContain<T>(this IEnumerable<T> collection, T expected, string message = "")
        {
            Assert.Contains(expected, new List<T>(collection), message);
        }

        public static void ShouldContain<T>(this IEnumerable<T> collection, T expected, string message, params object[] messageArguments)
        {
            Assert.Contains(expected, new List<T>(collection), String.Format(message, messageArguments));
        }

        public static void ShouldContain<T>(this IEnumerable<T> collection, Func<T, bool> matchMethod, string message = "")
        {
            bool hasFound = false;

            foreach (T item in collection)
            {
                if (matchMethod(item))
                {
                    hasFound = true;
                    break;
                }
            }

            if (!hasFound)
                Assert.Fail("No match in collection, using match method provided. {0}", message);
        }

        public static void ShouldContainAll<T>(this IEnumerable<T> collection, IEnumerable<T> checkCollection, string message = "")
        {
            IEnumerable<T> unionCollection = collection.Union(checkCollection);
            unionCollection.Count().ShouldEqual(collection.Count(), String.Format("Collections do not contain the same elements. {0}", message).Trim());
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> collection, T expected, string message = "")
        {
            Assert.False(collection.Contains(expected), "Should not contain failed. {0}", message);
        }
    }
}
