using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Jumbleblocks.Testing
{
    public static partial class TestHelpers
    {
        public static void ShouldBeWithinLast(this DateTime? dateTime, TimeSpan timeSpan, string message="")
        {
            dateTime.HasValue.ShouldBeTrue();

            bool isWithinTime = dateTime > (DateTime.Now - timeSpan);

            Assert.IsTrue(isWithinTime, "datetime falls before check value. {0}", message);
        }

        public static void ShouldNotBeWithinLast(this DateTime? dateTime, TimeSpan timeSpan, string message = "")
        {
            dateTime.HasValue.ShouldBeTrue();

            bool isWithinTime = dateTime < (DateTime.Now - timeSpan);

            Assert.IsTrue(isWithinTime, "datetime falls within check value. {0}", message);
        }
    }
}
