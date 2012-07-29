using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Web.Wane.ParseRules.Properties
{
    /// <summary>
    /// Parse rule for a style class CSS "Class"
    /// </summary>
    public class StyleClassParseRule : PropertyParseRule
    {
        public StyleClassParseRule()
            : base(NameOfProperty, "class")
        {
        }

        /// <summary>
        /// The property name which this parse rule operates for
        /// </summary>
        public static string NameOfProperty { get { return "StyleClass"; } }
    }
}
