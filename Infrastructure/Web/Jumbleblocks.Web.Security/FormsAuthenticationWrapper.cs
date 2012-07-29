using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Core.Security;
using System.Web;
using System.Web.Security;
using Jumbleblocks.Core.Configuration;
using Jumbleblocks.Web.Security.Configuration;

namespace Jumbleblocks.Web.Security
{
    /// <summary>
    /// Authenticate user, using a cookie
    /// </summary>
    public class FormsAuthenticationWrapper : IFormsAuthentication
    {
        public FormsAuthenticationWrapper(IConfigurationReader configurationReader)
        {
            if (configurationReader == null)
                throw new ArgumentNullException();

            ConfigurationReader = configurationReader;
        }

        public IConfigurationReader ConfigurationReader { get; private set; }
        protected SecurityConfigurationSection SecurityConfig { get { return ConfigurationReader.GetSection<SecurityConfigurationSection>("Jumbleblocks.Security"); } }
        

        public HttpContextBase HttpContextBase { get { return new HttpContextWrapper(HttpContext.Current); } }

        /// <summary>
        /// Sets the auth cookie with the user name
        /// </summary>
        /// <param name="principal">principal to get data from</param>
        public void SetAuth(IJumbleblocksPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated)
            {
                var slidingExpiration = SecurityConfig.GetCookieSlidingExpiration();

                var ticket = new FormsAuthenticationTicket(1,
                                                           principal.Identity.Name,
                                                           DateTime.Now,
                                                           DateTime.Now.Add(slidingExpiration),
                                                           false,
                                                           String.Empty,
                                                           FormsAuthentication.FormsCookiePath);

                string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                HttpContextBase.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
            }
        }

        /// <summary>
        /// gets the username from the auth cookie
        /// </summary>
        /// <param name="httpContext">HttpContextBase</param>
        /// <returns>string [username], String.Empty if no forms authentication cookie</returns>
        public string GetUsername()
        {
            var username = String.Empty;

            if (HttpContextBase.Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
            {
                string encryptedTicket = HttpContextBase.Request.Cookies[FormsAuthentication.FormsCookieName].Value;
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(encryptedTicket);

                username = ticket.Name;
            }

            return username;
        }

        /// <summary>
        /// sets auth cookie, so user is signed out
        /// </summary>
        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
