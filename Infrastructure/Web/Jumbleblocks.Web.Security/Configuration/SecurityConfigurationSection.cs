using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Jumbleblocks.Web.Security.Configuration
{
    public class SecurityConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("cookieSlidingExpiration")]
        public SlidingExpirationElement CookieSlidingExpiration
        {
            get { return this["cookieSlidingExpiration"] as SlidingExpirationElement; }
            set { this["cookieSlidingExpiration"] = value; }
        }

        [ConfigurationProperty("cacheSlidingExpiration")]
        public SlidingExpirationElement CacheSlidingExpiration
        {
            get { return this["cacheSlidingExpiration"] as SlidingExpirationElement; }
            set { this["cacheSlidingExpiration"] = value; }
        }

        public TimeSpan GetCookieSlidingExpiration()
        {
            return GetSlidingExpiration(CookieSlidingExpiration);
        }

        public TimeSpan GetCacheSlidingExpiration()
        {
            return GetSlidingExpiration(CacheSlidingExpiration);
        }
        
        private TimeSpan GetSlidingExpiration(SlidingExpirationElement slidingExpiration)
        {
            if (slidingExpiration == null)
                return TimeSpan.Zero;
            else
                return new TimeSpan(slidingExpiration.Hours, slidingExpiration.Minutes, slidingExpiration.Seconds);
        }
    }
}
