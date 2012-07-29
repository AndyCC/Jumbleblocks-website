using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Jumbleblocks.Website.Configuration
{
    public class AcceptedRedirectUrlElement : ConfigurationElement
    {
        /// <summary>
        /// Url which is accepted
        /// </summary>
        [ConfigurationProperty("url", IsRequired = true, DefaultValue="http://")]
        [RegexStringValidator(@"^https?://")]
        public string Url
        {
            get 
            { 
                return (string)this["url"];
            }
            set 
            {
                var uri = new Uri(value, UriKind.Absolute);
                this["url"] = uri.AbsoluteUri; 
            }
        }
    }
}