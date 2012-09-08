using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Web.Wane.ParseRules.Properties
{
    /// <summary>
    /// parse href
    /// </summary>
    public class HrefParseRule : PropertyParseRule
    {
        public HrefParseRule()
            : base(NameOfProperty, "href")
        {
        }

        /// <summary>
        /// The property name which this parse rule operates for
        /// </summary>
        public static string NameOfProperty { get { return "Url"; } }
    }
}
