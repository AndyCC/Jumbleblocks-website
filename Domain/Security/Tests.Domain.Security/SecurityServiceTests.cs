using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jumbleblocks.Testing;
using Jumbleblocks.Domain.Security;
using Jumbleblocks.Core;
using Moq;
using System.Security.Cryptography;
using Jumbleblocks.Core.Cryptography;

namespace Tests.Jumbleblocks.Domain.Security
{
    [TestClass]
    public class SecurityServiceTests
    {
       [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_WHEN_userRepository_Is_Null_THEN_Throws_ArgumentNullException()
        {
            new SecurityService(null, new Mock<IStringHasher>().Object);
        }

       [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_WHEN_passwordHasher_Is_Null_THEN_Throws_ArgumentNullException()
        {
            new SecurityService(new Mock<IUserRepository>().Object, null);
        }

       [TestMethod]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void RetrievePrincipal_WHEN_Username_Is_Null_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            var mockedRepository = new Mock<IUserRepository>();
            var securityService = new SecurityService(mockedRepository.Object, new Mock<IStringHasher>().Object);

            securityService.RetrievePrincipal(null, "abc");
        }

       [TestMethod]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void RetrievePrincipal_WHEN_Username_Is_EmptyString_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            var mockedRepository = new Mock<IUserRepository>();
            var securityService = new SecurityService(mockedRepository.Object, new Mock<IStringHasher>().Object);

            securityService.RetrievePrincipal(String.Empty, "abc");
        }

       [TestMethod]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void RetrievePrincipal_WHEN_Password_Is_Null_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            var mockedRepository = new Mock<IUserRepository>();
            var securityService = new SecurityService(mockedRepository.Object, new Mock<IStringHasher>().Object);

            securityService.RetrievePrincipal("abc", null);
        }

       [TestMethod]
        [ExpectedException(typeof(StringArgumentNullOrEmptyException))]
        public void RetrievePrincipal_WHEN_Password_Is_EmptyString_THEN_Throws_StringArgumentNullOrEmptyException()
        {
            var mockedRepository = new Mock<IUserRepository>();
            var securityService = new SecurityService(mockedRepository.Object, new Mock<IStringHasher>().Object);

            securityService.RetrievePrincipal("abc", String.Empty);
        }

       [TestMethod]
        public void RetrievePrincipal_GIVEN_UserRepository_LoadForUsername_Returns_Null_THEN_Returns_Principal_With_Identity_JumbleblocksAnonymousIdentity()
        {
            var mockedRepository = new Mock<IUserRepository>();
            mockedRepository.Setup((r) => r.LoadForUsernameAndPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(null as User);

            var securityService = new SecurityService(mockedRepository.Object, new MD5StringHasher());

            var principal = securityService.RetrievePrincipal("abc", "def");

            principal.Identity.ShouldBeInstanceOfType(typeof(JumbleblocksAnonymousIdentity));
        }

       [TestMethod]
        public void RetrievePrincipal_GIVEN_User_COntains_User_With_Username_ABC_Password_That_Is_An_MD5_Hash_Of_DEF_WHEN_Username_Is_ABC_AND_Password_DEF_THEN_Returns_Principal_With_Identity_Of_Type_JumbleblocksUserIdentity_With_Username_ABC()
        {
            const string Username = "ABC";
            const string Password = "DEF";

            var hasher = new MD5StringHasher();
            string hashedPassword = hasher.GetHash(Password);

            var user = new User();
            user.SetProperty(u => u.Id, 1);
            user.Username = Username;

            var mockedRepository = new Mock<IUserRepository>();
            mockedRepository.Setup((r) => r.LoadForUsernameAndPassword(It.IsAny<string>(), hashedPassword)).Returns(user);

            var securityService = new SecurityService(mockedRepository.Object, hasher);

            var principal = securityService.RetrievePrincipal(Username, Password);

            principal.Identity.ShouldBeInstanceOfType(typeof(JumbleblocksUserIdentity));
            principal.Identity.Name.ShouldEqual(Username);
        }

       [TestMethod]
        public void RetrievePrincipal_GIVEN_User_Contains_User_With_Username_ABC_Password_That_Is_An_MD5_Hash_Of_DEF_WHEN_Username_Is_ABC_AND_Password_EFG_THEN_Returns_Principal_With_Identity_Of_Type_JumbleblocksAnonymousIdentity()
        {
            const string Username = "ABC";
            const string Password = "DEF";

            var hasher = new MD5StringHasher();
            string hashedPassword = hasher.GetHash(Password);

            var user = new User();
            user.SetProperty(u => u.Id, 1);
            user.Username = Username;

            var mockedRepository = new Mock<IUserRepository>();
            mockedRepository.Setup((r) => r.LoadForUsernameAndPassword(It.IsAny<string>(), hashedPassword)).Returns(user);

            var securityService = new SecurityService(mockedRepository.Object, hasher);

            var principal = securityService.RetrievePrincipal(Username, "EFG");

            principal.Identity.ShouldBeInstanceOfType(typeof(JumbleblocksAnonymousIdentity));
        }
    }
}
