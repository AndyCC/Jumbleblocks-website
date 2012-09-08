using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Web.Wane.ParseRules.Properties
{
    public class AltParseRule : PropertyParseRule
    {
        public AltParseRule()
            : base(NameOfProperty, "alt")
        {
        }

        /// <summary>
        /// The property name which this parse rule operates for
        /// </summary>
        public static string NameOfProperty { get { return "Alt"; } }
    }
}
