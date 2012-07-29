using System;
using System.Web;
using Jumbleblocks.Core.Security;

namespace Jumbleblocks.Web.Security
{
    /// <summary>
    /// interface for cookie authenticator
    /// </summary>
    public interface IFormsAuthentication
    {
        /// <summary>
        /// Sets the authentication cookie
        /// </summary>
        /// <param name="principal">principal to use</param>
        void SetAuth(IJumbleblocksPrincipal principal);
      
        /// <summary>
        /// gets the username from the auth cookie
        /// </summary>
        /// <param name="httpContext">HttpContextBase</param>
        /// <returns>string [username], String.Empty if no forms authentication cookie</returns>
        string GetUsername();

        /// <summary>
        /// Signs out
        /// </summary>
        void LogOut();
    }
}
