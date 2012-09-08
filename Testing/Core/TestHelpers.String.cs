using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jumbleblocks.Testing
{
    public static partial class TestHelpers
    {
        public static void ShouldNotBeNullOrEmpty(this string str, string message = "")
        {
            Assert.IsNotNull(str, message);
            Assert.AreNotEqual(str, String.Empty);
        }

        public static void ShouldEqual(this string str, string expected, StringComparison stringComparison, string message = "")
        {
            if(!String.Equals(str, expected, stringComparison))
                Assert.Fail("The following do not match using string comparison '{0}':\n {1} [Actual]\b {1} [Expected]", stringComparison.ToString(), str, expected); 
        }

        public static void ShouldStartWith(this string str, string expected, StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase, string message = "")
        {
           Assert.IsTrue(str.StartsWith(expected, stringComparison), 
               String.Format("The following do not match start of string using string comparison '{0}':\n {1} [Actual]\b {1} [Expected]", stringComparison.ToString(), str, expected));
        }
    }
}
