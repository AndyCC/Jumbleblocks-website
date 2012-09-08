using Jumbleblocks.Web.Wane.ParseRules.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Web.Wane.ParseRules.Delimeters
{
    public class AnchorParseRule : DelimeterParseRule
    {
        public AnchorParseRule()
            : base("Anchor", DefaultDelimeterValues.DelimeterStart + "a", "a")
        {
             AddPropertyParseRule(new HrefParseRule());
        }
    }
}
