using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jumbleblocks.Testing;
using Jumbleblocks.Domain.Security;
using Moq;
using Jumbleblocks.Core.Security;

namespace Tests.Jumbleblocks.Domain.Security
{
    [TestClass]
    public class JumbleblocksPrincipalTests
    {
       [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_WHEN_identity_Is_Null_THEN_Throws_ArgumentNullException()
        {
            new JumbleblocksPrincipal(null);
        }


       [TestMethod]
        public void Ctor_Sets_Identity_Property_To_identity()
        {
            var identity = new JumbleblocksAnonymousIdentity();
            var principal = new JumbleblocksPrincipal(identity, null);

            principal.Identity.ShouldEqual(identity);
        }

       [TestMethod]
        public void IsInRole_Given_Ctor_user_Is_Null_THEN_Returns_False()
        {
            var principal = new JumbleblocksPrincipal(new JumbleblocksAnonymousIdentity(), null);
            var result = principal.IsInRole("abc");

            result.ShouldBeFalse();
        }

       [TestMethod]
        public void IsInRole_GIVEN_Has_Role_ABC_WHEN_roleName_IS_ABC_THEN_Returns_True()
        {
            const string RoleName = "ABC";

            var role = new Role { Name = RoleName };
            var user = new User();
            user.SetProperty(u => u.Id, 1);
            user.AddRole(role);
            
            var principal = new JumbleblocksPrincipal(new JumbleblocksUserIdentity(user), user);

            var result = principal.IsInRole(RoleName);

            result.ShouldBeTrue();
        }

       [TestMethod]
        public void IsInRole_GIVEN_Has_Role_ABC_WHEN_roleName_IS_DEF_THEN_Returns_False()
        {
            const string RoleName = "ABC";
            const string CheckRoleName = "DEF";

            var role = new Role { Name = RoleName };
            var user = new User();
            user.SetProperty(u => u.Id, 1);
            user.AddRole(role);

            var principal = new JumbleblocksPrincipal(new JumbleblocksUserIdentity(user), user);

            var result = principal.IsInRole(CheckRoleName);

            result.ShouldBeFalse();
        }


       [TestMethod]
        public void HasOperation_GIVEN_Ctor_user_Is_Null_THEN_Returns_False()
        {
            var principal = new JumbleblocksPrincipal(new JumbleblocksAnonymousIdentity(), null);
            var result = principal.HasOperation("abc");

            result.ShouldBeFalse();
        }

       [TestMethod]
        public void HasOperation_GIVEN_Has_Operation_ABC_WHEN_operationName_IS_ABC_THEN_Returns_True()
        {
            const string OperationName = "ABC";

            var role = new Role { Name = "role" };
            role.AddOperation(new Operation { Name = OperationName });

            var user = new User();
            user.SetProperty(u => u.Id, 1);
            user.AddRole(role);

            var principal = new JumbleblocksPrincipal(new JumbleblocksUserIdentity(user), user);

            var result = principal.HasOperation(OperationName);

            result.ShouldBeTrue();
        }


       [TestMethod]
        public void HasOperation_GIVEN_Has_Operation_ABC_WHEN_operationName_IS_DEF_THEN_Returns_False()
        {
            const string OperationName = "ABC";
            const string CheckOperationName = "DEF";

            var role = new Role { Name = "role" };
            role.AddOperation(new Operation { Name = OperationName });

            var user = new User();
            user.SetProperty(u => u.Id, 1);
            user.AddRole(role);

            var principal = new JumbleblocksPrincipal(new JumbleblocksUserIdentity(user), user);

            var result = principal.HasOperation(CheckOperationName);

            result.ShouldBeFalse();
        }

       [TestMethod]
        public void CanPerformRole_GIVEN_Ctor_user_Is_Null_THEN_Returns_False()
        {
            var mockedIdentity = new Mock<IJumbleblocksIdentity>();
            mockedIdentity.Setup(i => i.IsAuthenticated).Returns(true);

            var principal = new JumbleblocksPrincipal(mockedIdentity.Object, null);

            var result = principal.CanPerformRole("ABC");

            result.ShouldBeFalse();
        }

       [TestMethod]
        public void CanPerformRole_GIVEN_Ctor_user_Has_Role_But_Identity_Is_Not_Authenticated_THEN_Returns_False()
        {
            const string RoleName = "ABC";

            var role = new Role { Name = RoleName };
            var user = new User();
            user.SetProperty(u => u.Id, 1);
            user.AddRole(role);
            
            var mockedIdentity = new Mock<IJumbleblocksIdentity>();
            mockedIdentity.Setup(i => i.IsAuthenticated).Returns(false);

            var principal = new JumbleblocksPrincipal(mockedIdentity.Object, user);

            var result = principal.CanPerformRole(RoleName);

            result.ShouldBeFalse();
        }

       [TestMethod]
        public void CanPerformRole_GIVEN_Ctor_user_Has_Role_And_Identity_Is_Authenticated_THEN_Returns_True()
        {
            const string RoleName = "ABC";

            var role = new Role { Name = RoleName };
            var user = new User();
            user.SetProperty(u => u.Id, 1);
            user.AddRole(role);

            var mockedIdentity = new Mock<IJumbleblocksIdentity>();
            mockedIdentity.Setup(i => i.IsAuthenticated).Returns(true);

            var principal = new JumbleblocksPrincipal(mockedIdentity.Object, user);

            var result = principal.CanPerformRole(RoleName);

            result.ShouldBeTrue();
        }

       [TestMethod]
        public void CanPerformRole_GIVEN_Ctor_user_Does_Not_Have_Role_And_Identity_Is_Authenticated_THEN_Returns_False()
        {
            const string RoleName = "ABC";

            var role = new Role { Name = "DEF" };
            var user = new User();
            user.SetProperty(u => u.Id, 1);
            user.AddRole(role);

            var mockedIdentity = new Mock<IJumbleblocksIdentity>();
            mockedIdentity.Setup(i => i.IsAuthenticated).Returns(true);

            var principal = new JumbleblocksPrincipal(mockedIdentity.Object, user);

            var result = principal.CanPerformRole(RoleName);

            result.ShouldBeFalse();
        }

       [TestMethod]
        public void CanPerformOperation_GIVEN_Ctor_user_Is_Null_THEN_Returns_False()
        {
            var mockedIdentity = new Mock<IJumbleblocksIdentity>();
            mockedIdentity.Setup(i => i.IsAuthenticated).Returns(true);

            var principal = new JumbleblocksPrincipal(mockedIdentity.Object, null);

            var result = principal.CanPerformOperation("ABC");

            result.ShouldBeFalse();
        }

       [TestMethod]
        public void CanPerformOperation_GIVEN_Ctor_user_Has_Operation_But_Identity_Is_Not_Authenticated_THEN_Returns_False()
        {
            const string OperationName = "ABC";

            var operation = new Operation { Name = OperationName };
            var role = new Role { Name = "role" };
            role.AddOperation(operation);

            var user = new User();
            user.SetProperty(u => u.Id, 1);
            user.AddRole(role);

            var mockedIdentity = new Mock<IJumbleblocksIdentity>();
            mockedIdentity.Setup(i => i.IsAuthenticated).Returns(false);

            var principal = new JumbleblocksPrincipal(mockedIdentity.Object, user);

            var result = principal.CanPerformOperation(OperationName);

            result.ShouldBeFalse();
        }

       [TestMethod]
        public void CanPerformOperation_GIVEN_Ctor_user_Has_Operation_And_Identity_Is_Authenticated_THEN_Returns_True()
        {
            const string OperationName = "ABC";

            var operation = new Operation { Name = OperationName };
            var role = new Role { Name = "role" };
            role.AddOperation(operation);

            var user = new User();
            user.SetProperty(u => u.Id, 1);
            user.AddRole(role);

            var mockedIdentity = new Mock<IJumbleblocksIdentity>();
            mockedIdentity.Setup(i => i.IsAuthenticated).Returns(true);

            var principal = new JumbleblocksPrincipal(mockedIdentity.Object, user);

            var result = principal.CanPerformOperation(OperationName);

            result.ShouldBeTrue();
        }

       [TestMethod]
        public void CanPerformOperation_GIVEN_Ctor_user_Does_Not_Have_Operation_And_Identity_Is_Authenticated_THEN_Returns_False()
        {
            const string OperationName = "ABC";

            var operation = new Operation { Name = "DEF" };
            var role = new Role { Name = "role" };
            role.AddOperation(operation);

            var user = new User();
            user.SetProperty(u => u.Id, 1);
            user.AddRole(role);

            var mockedIdentity = new Mock<IJumbleblocksIdentity>();
            mockedIdentity.Setup(i => i.IsAuthenticated).Returns(true);

            var principal = new JumbleblocksPrincipal(mockedIdentity.Object, user);

            var result = principal.CanPerformOperation(OperationName);

            result.ShouldBeFalse();
        }
    }
}
