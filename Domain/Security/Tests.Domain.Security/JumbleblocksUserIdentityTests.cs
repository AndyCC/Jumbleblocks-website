using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Jumbleblocks.Testing;
using Jumbleblocks.Domain.Security;
using Jumbleblocks.Core;

namespace Tests.Jumbleblocks.Domain.Security
{
    [TestFixture]
    public class JumbleblocksUserIdentityTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_WHEN_user_Is_Null_THEN_Throws_ArgumentNullException()
        {
            new JumbleblocksUserIdentity(null);
        }

        [Test]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void Ctor_WHEN_authenticationType_Is_Null_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            new JumbleblocksUserIdentity(new User(), null);
        }

        [Test]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void Ctor_WHEN_authenticationType_Is_EmptyString_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            new JumbleblocksUserIdentity(new User(), String.Empty);
        }

        [Test]
        public void Ctor_WHEN_authenticationType_IS_ABC_THEN_Sets_AuthenticationType_Property_To_ABC()
        {
            const string AuthenticationType = "ABC";
            var identity = new JumbleblocksUserIdentity(new User(), AuthenticationType);

            identity.AuthenticationType.ShouldEqual(AuthenticationType);    
        }

        [Test]
        public void Ctor_WHEN_User_Has_Null_Id_THEN_Sets_IsAuthenticated_To_False()
        {
            var user = new User();
            var identity = new JumbleblocksUserIdentity(user);

            identity.IsAuthenticated.ShouldBeFalse();
        }

        [Test]
        public void Ctor_WHEN_User_Has_An_Id_THEN_Sets_IsAuthenticated_To_True()
        {
            var user = new User();
            user.SetProperty(u => u.Id, 1);

            var identity = new JumbleblocksUserIdentity(user);

            identity.IsAuthenticated.ShouldBeTrue();
        }

        [Test]
        public void Ctor_WHEN_User_Has_UserName_Joe_THEN_Name_Property_Returns_Joe()
        {
            const string UserName = "Joe";

            var user = new User { Username = UserName };
            user.SetProperty(u => u.Id, 1);

            var identity = new JumbleblocksUserIdentity(user);

            identity.Name.ShouldEqual(UserName);
        }
    }
}
