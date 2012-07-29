﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Jumbleblocks.Testing
{
    public static partial class TestHelpers
    {
        public static void ShouldNotBeNullOrEmpty(this string str, string message = "")
        {
            Assert.IsNotNullOrEmpty(str, message);
        }

        public static void ShouldEqual(this string str, string expected, StringComparison stringComparison, string message = "")
        {
            if(!String.Equals(str, expected, stringComparison))
                Assert.Fail("The following do not match using string comparison '{0}':\n {1} [Actual]\b {1} [Expected]", stringComparison.ToString(), str, expected); 
        }       
    }
}