using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Jumbleblocks.Website.Configuration
{
    [ConfigurationCollection(typeof(AcceptedRedirectUrlElement), AddItemName="accepted")]
    public class AcceptedRedirectUrlCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AcceptedRedirectUrlElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return ((AcceptedRedirectUrlElement)element).Url;
        }

        /// <summary>
        /// Default redirect url to use when invalid redirect url supplied
        /// </summary>
        [ConfigurationProperty("authenticationDefaultRedirectUrl", IsRequired = true)]
        public string AuthenticationDefaultRedirectUrl
        {
            get { return (string)this["authenticationDefaultRedirectUrl"]; }
            set
            {
                var uri = new Uri(value, UriKind.RelativeOrAbsolute);
                this["authenticationDefaultRedirectUrl"] = uri.ToString(); 
            }
        }

    }
}