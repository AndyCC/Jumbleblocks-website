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
    public class UserTests
    {
        [Test]
        public void AddRole_Adds_To_Roles_List()
        {
            var role = new Role { Name = "Role" };

            var user = new User();
            user.AddRole(role);

            user.Roles.ShouldContain(role);
        }

        [Test]
        public void AddRole_GIVEN_User_Has_Role_WHEN_Adding_Same_Role_Again_THEN_Roles_Count_Remains_1()
        {
            var role = new Role { Name = "Role" };

            var user = new User();
            user.AddRole(role);
            user.AddRole(role);

            user.Roles.ShouldContain(role);
            user.Roles.Count().ShouldEqual(1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddRole_WHEN_role_Is_Null_THEN_Throws_ArgumentNullException()
        {
            var user = new User();
            user.AddRole(null);
        }

        [Test]
        public void HasRole_GIVEN_User_Has_No_Roles_WHEN_roleName_Is_ABC_THEN_Returns_False()
        {
            var user = new User();
            var result = user.HasRole("ABC");

            result.ShouldBeFalse();
        }

        [Test]
        public void HasRole_GIVEN_User_Has_Role_With_Name_ABC_WHEN_roleName_Is_ABC_THEN_Returns_True()
        {
            const string RoleName = "ABC";

            var role = new Role { Name = RoleName };
            var user = new User();
            user.AddRole(role);

            var result = user.HasRole(RoleName);

            result.ShouldBeTrue();
        }

        [Test]
        public void HasRole_GIVEN_User_Has_Role_With_Name_ABC_WHEN_roleName_Is_DEF_THEN_Returns_False()
        {
            const string RoleName = "ABC";
            const string CheckForRoleName = "DEF";

            var role = new Role { Name = RoleName };
            var user = new User();
            user.AddRole(role);

            var result = user.HasRole(CheckForRoleName);

            result.ShouldBeFalse();
        }

        [Test]
        public void HasOperation_GIVEN_User_Has_No_Roles_WHEN_operationName_Is_ABC_THEN_Returns_False()
        {
            const string CheckForOperationName = "ABC";

            var user = new User();

            var result = user.HasOperation(CheckForOperationName);

            result.ShouldBeFalse();
        }

        [Test]
        public void HasOperation_GIVEN_User_Has_1_Role_With_No_Operations_WHEN_operationName_Is_ABC_THEN_Returns_False()
        {
            const string CheckForOperationName = "ABC";

            var role = new Role { Name = "Role" };
            var user = new User();
            user.AddRole(role);

            var result = user.HasOperation(CheckForOperationName);

            result.ShouldBeFalse();
        }

        [Test]
        public void HasOperation_GIVEN_User_Has_1_Role_With_1_Operation_Named_ABC_WHEN_operationName_Is_ABC_THEN_Returns_True()
        {
            const string OperationName = "ABC";

            var operation = new Operation { Name = OperationName };

            var role = new Role { Name = "Role" };
            role.AddOperation(operation);

            var user = new User();
            user.AddRole(role);

            var result = user.HasOperation(OperationName);

            result.ShouldBeTrue();
        }
    }
}
