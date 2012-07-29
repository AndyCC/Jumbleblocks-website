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
    public class RoleTests
    {
        [Test]
        public void AddOperation_Add_Operation_To_List()
        {
            var operation = new Operation { Name = "Operation" };
            var role = new Role { Name = "Role" };

            role.AddOperation(operation);
            role.Operations.ShouldContain(operation);
        }

        [Test]
        public void AddOperation_GIVEN_Operation_Already_Added_WHEN_Added_Again_THEN_Does_Not_Add_To_List_Again()
        {
            var operation = new Operation { Name = "Operation" };
            var role = new Role { Name = "Role" };

            role.AddOperation(operation);
            role.AddOperation(operation);

            role.Operations.ShouldContain(operation);
            role.Operations.Count().ShouldEqual(1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddOperation_WHEN_operation_Is_Null_THEN_Throws_ArgumentNullException()
        {
            var role = new Role { Name = "Role" };

            role.AddOperation(null);
        }


        [Test]
        public void HasOperation_GIVEN_User_Has_No_Operations_WHEN_OperationName_Is_ABC_THEN_Returns_False()
        {
            var role = new Role { Name = "Role" };

            var result = role.HasOperation("ABC");

            result.ShouldBeFalse();
        }

        [Test]
        public void HasOperation_GIVEN_User_Has_Operation_With_Name_ABC_WHEN_operationName_Is_ABC_THEN_Returns_True()
        {
            const string OperationName = "ABC";

            var operation = new Operation { Name = OperationName };
            var role = new Role { Name = "Role" };
            role.AddOperation(operation);

            var result = role.HasOperation(OperationName);

            result.ShouldBeTrue();
        }

        [Test]
        public void HasOperation_GIVEN_User_Has_Operation_With_Name_DEF_WHEN_operationName_Is_ABC_THEN_Returns_False()
        {
            var operation = new Operation { Name = "DEF" };
            var role = new Role { Name = "Role" };
            role.AddOperation(operation);

            var result = role.HasOperation("ABC");

            result.ShouldBeFalse();
        }

        [Test]
        public void NameEquals_GIVEN_Role_Name_Is_ABC_WHEN_roleName_Is_ABC_THEN_Returns_True()
        {
            const string RoleName = "ABC";
            var role = new Role { Name = RoleName };

            var result = role.NameEquals(RoleName);

            result.ShouldBeTrue();
        }

        [Test]
        public void NameEquals_GIVEN_Role_Name_Is_ABC_WHEN_roleName_Is_abc_THEN_Returns_False()
        {
            const string RoleName = "ABC";
            const string CheckForRoleName = "abc";

            var role = new Role { Name = RoleName };

            var result = role.NameEquals(CheckForRoleName);

            result.ShouldBeFalse();
        }

        [Test]
        public void NameEquals_GIVEN_Role_Name_Is_ABC_WHEN_roleName_Is_DEF_THEN_Returns_False()
        {
            const string RoleName = "ABC";
            const string CheckForRoleName = "DEF";

            var role = new Role { Name = RoleName };

            var result = role.NameEquals(CheckForRoleName);

            result.ShouldBeFalse();
        }
    }
}
