using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Core.Security;
using System.Web;
using System.Security.Principal;
using Jumbleblocks.Domain.Security;
using System.Threading;

namespace Jumbleblocks.Web.Security
{
    /// <summary>
    /// Authenticator to use to authenticate web access
    /// </summary>
    public class WebAuthenticator : IWebAuthenticator 
    {
        public WebAuthenticator(IJumbleblocksSecurityService securityService, IFormsAuthentication formsAuthentication, IUserCache userCache)
        {
            if (securityService == null)
                throw new ArgumentNullException("securityService");

            if (userCache == null)
                throw new ArgumentNullException("userCache");

            if (formsAuthentication == null)
                throw new ArgumentNullException("formsAuthentication");

            SecurityService = securityService;
            FormsAuthentication = formsAuthentication;
            UserCache = userCache;
        }

        public IJumbleblocksSecurityService SecurityService { get; private set; }
        public IFormsAuthentication FormsAuthentication { get; private set; }
        public IUserCache UserCache { get; private set; }  
        
        public HttpContextBase HttpContextBase { get { return new HttpContextWrapper(HttpContext.Current); } }

        /// <summary>
        /// Authenticates a user
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>principal for user</returns>
        public IJumbleblocksPrincipal Authenticate(string username, string password)
        {
            if (String.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException("username");

            if (String.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("password");

            IJumbleblocksPrincipal principal = SecurityService.RetrievePrincipal(username, password);

            if (principal.Identity.IsAuthenticated)
            {
                UserCache.AddPrincipalToCache(principal); 
                SetCurrentPrincipal(principal);
            }

            return principal;
        }

        private void SetCurrentPrincipal(IJumbleblocksPrincipal principal)
        {
            FormsAuthentication.SetAuth(principal);
            HttpContextBase.User = principal;
        }


        /// <summary>
        /// Retrieves the current users username from cookie
        /// </summary>
        /// <returns>username</returns>
        public string GetUsernameFromCookie()
        {
            return FormsAuthentication.GetUsername();
        }

        /// <summary>
        /// Ensures a given principal is a Jumbleblocks principal
        /// </summary>
        /// <param name="originalPrincipal">principal to check</param>
        /// <returns>IJumbleblocksPrincipal</returns>
        public IJumbleblocksPrincipal EnsureAuthenticatedAsJumbleblocksPrincipal(IPrincipal originalPrincipal)
        {
            if (originalPrincipal is IJumbleblocksPrincipal)
                return (IJumbleblocksPrincipal)originalPrincipal;

            string usernameInCookie = FormsAuthentication.GetUsername();

            if (!originalPrincipal.Identity.IsAuthenticated || !NameMatchesIdentity(originalPrincipal, usernameInCookie)) 
                return new JumbleblocksPrincipal(new JumbleblocksAnonymousIdentity());

            IJumbleblocksPrincipal principal = UserCache.GetPrincipalFromCache(usernameInCookie);

            if (principal == null)
            {
                principal = SecurityService.RetrievePrincipal(usernameInCookie);
                UserCache.AddPrincipalToCache(principal); 
            }

            SetCurrentPrincipal(principal);

            return principal;
        }

        private static bool NameMatchesIdentity(IPrincipal originalPrincipal, string usernameInCookie)
        {
            return usernameInCookie == originalPrincipal.Identity.Name;
        }

        /// <summary>
        /// Logs user out
        /// </summary>
        public void LogOut()
        {
            FormsAuthentication.LogOut();
            HttpContextBase.User = null;
        }
    }
}
