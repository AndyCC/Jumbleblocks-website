using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Core.Security;

namespace Jumbleblocks.Domain.Security
{
    /// <summary>
    /// Identity for an anonyous user
    /// </summary>
    public class JumbleblocksAnonymousIdentity : IJumbleblocksIdentity
    {
        public string AuthenticationType
        {
            get { return "Jumbleblocks"; }
        }

        public bool IsAuthenticated
        {
            get { return false; }
        }

        public int? UserId
        {
            get { return null; }
        }

        public string Name
        {
            get { return "Anonymous"; }
        }

        public string Forename
        {
            get { return String.Empty; }
        }

        public string Surname
        {
            get { return String.Empty; }
        }
    }
}
