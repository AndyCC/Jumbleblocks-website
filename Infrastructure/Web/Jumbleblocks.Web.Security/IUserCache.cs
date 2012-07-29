using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Core.Security;

namespace Jumbleblocks.Web.Security
{
    /// <summary>
    /// interface for a cache of users
    /// </summary>
    public interface IUserCache
    {
        /// <summary>
        /// Tries to get principal from cache
        /// </summary>
        /// <param name="username">username to check for</param>
        /// <returns>IJumbleblocksPrincipal</returns>
        IJumbleblocksPrincipal GetPrincipalFromCache(string username);

        /// <summary>
        /// Adds a principal to the cache
        /// </summary>
        /// <param name="principal">principal to use</param>
        void AddPrincipalToCache(IJumbleblocksPrincipal principal);

        /// <summary>
        /// tries to remove principal from cache
        /// </summary>
        /// <param name="principal">principal to use</param>
        void RemovePrincipalFromCache(IJumbleblocksPrincipal principal);
    }
}
