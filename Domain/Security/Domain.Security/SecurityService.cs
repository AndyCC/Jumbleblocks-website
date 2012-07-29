using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Core.Security;
using Jumbleblocks.Core;
using Jumbleblocks.Core.Cryptography;

namespace Jumbleblocks.Domain.Security
{
    public class SecurityService : IJumbleblocksSecurityService
    {
        public SecurityService(IUserRepository userRepository, IStringHasher passwordHasher)
        {
            if (userRepository == null)
                throw new ArgumentNullException("userRepository");

            if (passwordHasher == null)
                throw new ArgumentNullException("passwordHasher");

            UserRepository = userRepository;
            PasswordHasher = passwordHasher;
        }

        public IUserRepository UserRepository { get; private set; }
        public IStringHasher PasswordHasher { get; private set; }

        public IJumbleblocksPrincipal RetrievePrincipal(string username, string password)
        {
            if (String.IsNullOrWhiteSpace(username))
                throw new StringArgumentNullOrEmptyException("username");

            if (String.IsNullOrWhiteSpace(password))
                throw new StringArgumentNullOrEmptyException("password");

            string hashedPassword = PasswordHasher.GetHash(password);
            User user = UserRepository.LoadForUsernameAndPassword(username, hashedPassword);
            IJumbleblocksIdentity identity = CreateIdentity(user);

            return new JumbleblocksPrincipal(identity, user);
        }

        /// <summary>
        /// Retrieve IJumbleblocks principal
        /// </summary>
        /// <param name="username">username to retrieve principal for</param>
        /// <returns>IJumbleblocksPrincipal</returns>
        public IJumbleblocksPrincipal RetrievePrincipal(string username)
        {
            if (String.IsNullOrWhiteSpace(username))
                throw new StringArgumentNullOrEmptyException("username");

            User user = UserRepository.LoadForUsername(username);
            IJumbleblocksIdentity identity = CreateIdentity(user);

            return new JumbleblocksPrincipal(identity, user);
        }

        private static IJumbleblocksIdentity CreateIdentity(User user)
        {
            if (user == null)
                return  new JumbleblocksAnonymousIdentity();
            else
                return new JumbleblocksUserIdentity(user);
        }
    }
}
