using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Core.Security;
using System.Web;
using System.Web.Caching;
using Jumbleblocks.Core.Configuration;
using Jumbleblocks.Web.Security.Configuration;

namespace Jumbleblocks.Web.Security
{
    public class HttpContextUserCache : IUserCache
    {
        public HttpContextUserCache(IConfigurationReader configurationReader)
        {
            if (configurationReader == null)
                throw new ArgumentNullException();

            ConfigurationReader = configurationReader;
        }

        public IConfigurationReader ConfigurationReader { get; private set; }
        protected SecurityConfigurationSection SecurityConfig { get { return ConfigurationReader.GetSection<SecurityConfigurationSection>("Jumbleblocks.Security"); } }
        
        public HttpContextBase HttpContextBase { get { return new HttpContextWrapper(HttpContext.Current); } }

        public IJumbleblocksPrincipal GetPrincipalFromCache(string username)        
        {
            return HttpContextBase.Cache[username] as IJumbleblocksPrincipal;
        }

        public void AddPrincipalToCache(IJumbleblocksPrincipal principal)
        {
            var slidingExpiration = SecurityConfig.GetCacheSlidingExpiration();

            HttpContextBase.Cache.Add(principal.Identity.Name, principal, null, Cache.NoAbsoluteExpiration,
                                      slidingExpiration, CacheItemPriority.Normal, null);

        }

        public void RemovePrincipalFromCache(IJumbleblocksPrincipal principal)
        {
            HttpContextBase.Cache.Remove(principal.Identity.Name);
        }
    }
}
