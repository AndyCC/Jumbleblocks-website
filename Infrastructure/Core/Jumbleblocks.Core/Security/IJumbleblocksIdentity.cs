using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace Jumbleblocks.Core.Security
{
    /// <summary>
    /// interface for a basic jumbleblocks identity
    /// </summary>
    public interface IJumbleblocksIdentity : IIdentity
    {
        /// <summary>
        /// Id of user
        /// </summary>
        int? UserId { get; }

        /// <summary>
        /// Users forename
        /// </summary>
        string Forename { get; }

        /// <summary>
        /// Users surname
        /// </summary>
        string Surname { get; }
    }
}
