using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jumbleblocks.Testing
{
    public static partial class TestHelpers
    {
        public static void ShouldBeInstanceOfType<T>(this T obj, Type expectedType, string message = "")
        {
            Assert.IsInstanceOfType(obj, expectedType, message);
        }

        public static void ShouldEqual<T>(this T obj, T expected, string message = "")
        {
            Assert.AreEqual(expected, obj, message);
        }

        public static void ShouldNotEqual<T>(this T obj, T expected, string message = "")
        {
            Assert.AreNotEqual(expected, obj, message);
        }

        public static void ShouldNotBeNull<T>(this T obj, string message = "")
        {
            Assert.IsNotNull(obj, message);
        }

        public static void ShouldBeNull<T>(this T obj, string message = "")
        {
            Assert.IsNull(obj, message);
        }
    }
}
