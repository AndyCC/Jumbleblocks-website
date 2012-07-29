using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Jumbleblocks.Core.Security;
using Jumbleblocks.Web.Core;
using Jumbleblocks.Core.Logging;

namespace Jumbleblocks.Web.Security
{
    public class JumbleblocksSecurityHttpModule : IHttpModule
    {
        protected IWebAuthenticator WebAuthenticator { get { return IocContext.Container.Resolve<IWebAuthenticator>(); } }
        
        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += new EventHandler(context_PostAuthenticateRequest);
        }

        void context_PostAuthenticateRequest(object sender, EventArgs e)
        {
            PostAuthenticateRequest(sender as HttpApplication);
        }
        
        private void PostAuthenticateRequest(HttpApplication application)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);

            if (application.User != null && !(application.User is IJumbleblocksPrincipal))
            {     
                WebAuthenticator.EnsureAuthenticatedAsJumbleblocksPrincipal(httpContext.User);     
           

            }
        }

        public void Dispose()
        {
        }        
    }
}
