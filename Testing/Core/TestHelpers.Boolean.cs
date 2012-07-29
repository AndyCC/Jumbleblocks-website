using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Jumbleblocks.Testing
{
    public static partial class TestHelpers
    {
        public static void ShouldBeTrue(this bool b, string message = "")
        {
            Assert.IsTrue(b, message);
        }

        public static void ShouldBeFalse(this bool b, string message = "")
        {
            Assert.IsFalse(b, message);
        }
    }
}
