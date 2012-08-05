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
        [ConfigurationProperty("title", IsRequired=true)]
        public string Title
        {
            get { return (string)this["title"]; }
            set { this["title"] = value; }
        }

        /// <summary>
        /// The number of post summaries to show per page
        /// </summary>
        [ConfigurationProperty("pagePostSummaryCount", DefaultValue=10, IsRequired=false)]
        [IntegerValidator(MinValue=1, MaxValue=200)]
        public int PagePostSummaryCount
        {
            get { return (int)this["pagePostSummaryCount"]; }
            set { this["pagePostSummaryCount"] = value; }
        }

        /// <summary>
        /// default action
        /// </summary>
        [ConfigurationProperty("defaultAction", IsRequired = true)]
        public string DefaultAction
        {
            get { return (string)this["defaultAction"]; }
            set { this["defaultAction"] = value; }
        }

        /// <summary>
        /// default controller
        /// </summary>
        [ConfigurationProperty("defaultController", IsRequired = true)]
        public string DefaultController
        {
            get { return (string)this["defaultController"]; }
            set { this["defaultController"] = value; }
        }

        [ConfigurationProperty("titleImagePath", IsRequired = true)]
        public string TitleImagePath
        {
            get { return (string)this["titleImagePath"]; }
            set { this["titleImagePath"] = value; }
        }

        /// <summary>
        /// Gets the accepted redirect urls
        /// </summary>
        [ConfigurationProperty("redirectUrls", IsRequired=true)] 
        public AcceptedRedirectUrlCollection AcceptedRedirectUrls
        {
            get { return this["redirectUrls"] as AcceptedRedirectUrlCollection; }
            set { this["redirectUrls"] = value; }
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