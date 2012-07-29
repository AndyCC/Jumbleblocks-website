using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Core;
using Jumbleblocks.Core.Security;

namespace Jumbleblocks.Domain.Security
{
    /// <summary>
    /// Identity based upon a jumbleblocks user
    /// </summary>
    public class JumbleblocksUserIdentity : IJumbleblocksIdentity
    {
        public JumbleblocksUserIdentity(User user, string authenticationType = "Jumbleblocks")
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrWhiteSpace(authenticationType))
                throw new StringArgumentNullOrEmptyException("authenticationType");

            User = user;
            AuthenticationType = authenticationType;
        }

        protected User User { get; private set; }

        public int? UserId { get { return User.Id; } }

        public string Name
        {
            get { return User.Username; }
        }

        public string AuthenticationType { get; private set; }

        public bool IsAuthenticated
        {
            get { return User.Id.HasValue; }
        }

        public string Forename
        {
            get { return User.Forenames; }
        }

        public string Surname
        {
            get { return User.Surname; }
        }
    }
}
