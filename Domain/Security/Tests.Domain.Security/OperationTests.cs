using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Jumbleblocks.Testing;
using Jumbleblocks.Domain.Security;

namespace Tests.Jumbleblocks.Domain.Security
{
    [TestFixture]
    public class OperationTests
    {   
        [Test]
        public void NameEquals_GIVEN_Operations_Name_Is_ABC_WHEN_operationName_Is_ABC_THEN_Returns_True()
        {
            const string operationName = "ABC";

            var operation = new Operation { Name = operationName };

            var result = operation.NameEquals(operationName);

            result.ShouldBeTrue();
        }

        [Test]
        public void NameEquals_GIVEN_Operations_Name_Is_ABC_WHEN_operationName_Is_abc_THEN_Returns_False()
        {
            const string OperationName = "ABC";
            const string CheckForOperationName = "abc";

            var operation = new Operation { Name = OperationName };

            var result = operation.NameEquals(CheckForOperationName);

            result.ShouldBeFalse();
        }

        [Test]
        public void NameEquals_GIVEN_Operations_Name_Is_ABC_WHEN_operationName_Is_DEF_THEN_Returns_False()
        {
            const string OperationName = "ABC";
            const string CheckForOperationName = "DEF";

            var operation = new Operation { Name = OperationName };

            var result = operation.NameEquals(CheckForOperationName);

            result.ShouldBeFalse();
        }
    }
}
