using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Core.Security
{
    /// <summary>
    /// performs a number of security operations
    /// </summary>
    public interface IJumbleblocksSecurityService
    {
        /// <summary>
        /// Retrieves IJumbleblocks principal
        /// </summary>
        /// <param name="username">user name to retrieve with</param>
        /// <param name="password">password for user to authenticate with</param>
        /// <returns>IJumbleblocksPrincipal</returns>
        IJumbleblocksPrincipal RetrievePrincipal(string username, string password);

        /// <summary>
        /// Retrieve IJumbleblocks principal
        /// </summary>
        /// <param name="username">username to retrieve principal for</param>
        /// <returns>IJumbleblocksPrincipal</returns>
        IJumbleblocksPrincipal RetrievePrincipal(string username);
    }
}
