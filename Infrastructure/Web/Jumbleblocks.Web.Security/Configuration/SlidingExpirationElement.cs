using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Jumbleblocks.Web.Security.Configuration
{
   
    public class SlidingExpirationElement : ConfigurationElement
    {
        [ConfigurationProperty("hours", IsRequired = false, DefaultValue = 0)]
        [IntegerValidator]
        public int Hours
        {
            get { return (int)this["hours"]; }
            set { this["hours"] = value; }
        }

        [ConfigurationProperty("minutes", IsRequired = false, DefaultValue = 0)]
        [IntegerValidator]
        public int Minutes
        {
            get { return (int)this["minutes"]; }
            set { this["minutes"] = value; }
        }

        [ConfigurationProperty("seconds", IsRequired = false, DefaultValue = 0)]
        [IntegerValidator]
        public int Seconds
        {
            get { return (int)this["seconds"]; }
            set { this["seconds"] = value; }
        }
    }
}
