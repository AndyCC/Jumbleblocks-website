using System;
using Jumbleblocks.Core.Security;
using System.Security.Principal;

namespace Jumbleblocks.Web.Security
{
    /// <summary>
    /// interface for authenticator
    /// </summary>
    public interface IWebAuthenticator
    {

        /// <summary>
        /// Authenticates a user
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>principal for user</returns>
        IJumbleblocksPrincipal Authenticate(string username, string password);

        /// <summary>
        /// Retrieves the current users username from cookie
        /// </summary>
        /// <returns>username</returns>
        string GetUsernameFromCookie();

        /// <summary>
        /// Ensures a given principal is a Jumbleblocks principal
        /// </summary>
        /// <param name="originalPrincipal">principal to check</param>
        /// <returns>IJumbleblocksPrincipal</returns>
        IJumbleblocksPrincipal EnsureAuthenticatedAsJumbleblocksPrincipal(IPrincipal originalPrincipal);

        /// <summary>
        /// Logs user out
        /// </summary>
        void LogOut();
    }
}
