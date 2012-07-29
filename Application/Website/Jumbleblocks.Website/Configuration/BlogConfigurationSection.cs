using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Jumbleblocks.Website.Configuration
{
    /// <summary>
    /// Configuration Section for Blog Settings
    /// </summary>
    public class BlogConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// The title of the blog
        /// </summary>
        [ConfigurationProperty("Title", IsRequired=true)]
        public string Title
        {
            get { return (string)this["Title"]; }
            set { this["Title"] = value; }
        }

        /// <summary>
        /// The number of post summaries to show per page
        /// </summary>
        [ConfigurationProperty("PagePostSummaryCount", DefaultValue=10, IsRequired=false)]
        [IntegerValidator(MinValue=1, MaxValue=200)]
        public int PagePostSummaryCount
        {
            get { return (int)this["PagePostSummaryCount"]; }
            set { this["PagePostSummaryCount"] = value; }
        }

        /// <summary>
        /// default action
        /// </summary>
        [ConfigurationProperty("DefaultAction", IsRequired = true)]
        public string DefaultAction
        {
            get { return (string)this["DefaultAction"]; }
            set { this["DefaultAction"] = value; }
        }

        /// <summary>
        /// default controller
        /// </summary>
        [ConfigurationProperty("DefaultController", IsRequired = true)]
        public string DefaultController
        {
            get { return (string)this["DefaultController"]; }
            set { this["DefaultController"] = value; }
        }
       
        /// <summary>
        /// Gets the accepted redirect urls
        /// </summary>
        [ConfigurationProperty("RedirectUrls", IsRequired=true)] 
        public AcceptedRedirectUrlCollection AcceptedRedirectUrls
        {
            get { return this["RedirectUrls"] as AcceptedRedirectUrlCollection; }
            set { this["RedirectUrls"] = value; }
        }

        /// <summary>
        /// default redirect url
        /// </summary>
        public string AuthenticationDefaultRedirectUrl
        {
            get { return AcceptedRedirectUrls.AuthenticationDefaultRedirectUrl; }
        }

        /// <summary>
        /// Determines if url is acceptable as a redirect url
        /// </summary>
        /// <param name="url">url to validate</param>
        /// <returns>true if url starts with an acceptable url fragment, otherwise false</returns>
        public bool UrlStartWithAllowedRedirectUrl(string url)
        {
            foreach (AcceptedRedirectUrlElement element in AcceptedRedirectUrls)
            {
                if (url.StartsWith(element.Url, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}