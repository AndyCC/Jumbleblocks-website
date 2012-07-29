using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Core.Security;
using System.Runtime.Caching;

namespace Jumbleblocks.Web.Security
{
    public class MemoryUserCache : IUserCache
    {
        /// <summary>
        /// Cache to store logged in user details
        /// </summary>
        protected MemoryCache Cache { get { return MemoryCache.Default; } }

        /// <summary>
        /// The default cache policy for users
        /// </summary>
        private static readonly CacheItemPolicy _defaultCachePolicy = new CacheItemPolicy { SlidingExpiration = new TimeSpan(0, 2, 0) };

        /// <summary>
        /// retrieves principal from cache
        /// </summary>
        /// <param name="username">username of user</param>
        /// <returns>IJumbleblocksPrincipal or null if not in cache</returns>
        public IJumbleblocksPrincipal GetPrincipalFromCache(string username)
        {
            return Cache[username] as IJumbleblocksPrincipal;
        }

        /// <summary>
        /// Adds principal to cache
        /// </summary>
        /// <param name="principal">principal to add to cache</param>
        public void AddPrincipalToCache(IJumbleblocksPrincipal principal)
        {            
            if (principal != null)
            {
                var cacheItem = new CacheItem(principal.Identity.Name, principal);
                Cache.Add(cacheItem, _defaultCachePolicy);
            }
        }

        /// <summary>
        /// Removes principal from cache
        /// </summary>
        /// <param name="principal">principal to remove</param>
        public void RemovePrincipalFromCache(IJumbleblocksPrincipal principal)
        {
            Cache.Remove(principal.Identity.Name);
        }
    }
}
